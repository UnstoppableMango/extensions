#!/bin/bash

exec() {
  echo ">>> $@";

  errorActionPreference="Continue";

  $@;
  result="$?";
  
  errorActionPreference="Stop";
  if [[ $result != 0 ]]; then
    exit $result;
  fi
}
