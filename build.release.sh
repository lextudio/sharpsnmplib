#!/bin/bash

set -e

# Remove bin directory
if [ -d "bin" ]; then
    rm -rf bin
fi

# Run release PowerShell script
pwsh -ExecutionPolicy Bypass -File release.ps1

echo "succeeded."
exit 0
