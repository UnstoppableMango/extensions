#!/bin/bash

exec() {
  echo "\e[90m>>> $@";

  errorActionPreference="Continue";

  $@;
  result="$?";
  
  errorActionPreference="Stop";
  if [[ $result != 0 ]]; then
    echo "Failed with exit code $result"
    exit $result;
  fi
}
