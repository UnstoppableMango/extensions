#!/bin/bash

set -e

SOURCE="${BASH_SOURCE[0]}"
while [ -h "$SOURCE" ]; do # resolve $SOURCE until the file is no longer a symlink
  DIR="$( cd -P "$( dirname "$SOURCE" )" && pwd )"
  SOURCE="$(readlink "$SOURCE")"
  [[ "$SOURCE" != /* ]] && SOURCE="$DIR/$SOURCE" # if $SOURCE was a relative symlink, we need to resolve it relative to the path where the symlink file was located
done
DIR="$( cd -P "$( dirname "$SOURCE" )" && pwd )"
REPOROOT="$DIR"

args=( )
Architecture=
Configuration=

while [[ $# > 0 ]]; do
  lowerI="$(echo $1 | awk '{print tolower($0)}')"
  case $lowerI in
    -a|--architecture)
      Architecture="$2"
      args+=("/p:Architecture=$Architecture")
      shift
	    ;;
    -c|--configuration)
      Configuration="$2"
      shift
      ;;
    --ci)
      ci=true
      shift
      ;;
    *)
      args+=("$1")
      ;;
  esac

  shift
done

source "$REPOROOT/src/common.sh"

if [[ -z "${CI}" ]]; then
  ci=true;
fi

if [[ "$ci" == true ]]; then
  args+=("/p:CI=true");
fi

if [[ -z "$Configuration" ]]; then
  if [[ "$ci" == true ]]; then
    $Configuration="Release";
  else
    $Configuration="Debug";
  fi
fi

[ "$BUILD_REASON" == "PullRequest" ] || isPr=true

artifacts="$REPOROOT/artifacts/"

rm -r $artifacts
execute dotnet msbuild /t:UpdateCiSettings "${args[@]}"
execute dotnet build --configuration $Configuration "${args[@]}"
execute dotnet pack --no-restore --no-build --configuration $Configuration -o $artifacts "${args[@]}"

echo "\e[35mDone"
