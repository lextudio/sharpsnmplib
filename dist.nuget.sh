#!/bin/bash

set -e

# Remove old packages
rm -f SharpSnmpLib/bin/Release/*.nupkg
rm -f SharpSnmpLib/bin/Release/*.snupkg
rm -f *.nupkg
rm -f *.snupkg

# Build
./build.release.sh

# Note: Code signing is skipped on macOS (not required for NuGet package creation)
# If signing is needed, run sign3.ps1 and sign.nuget.ps1 on Windows

# Copy packages to root
cp SharpSnmpLib/bin/Release/*.nupkg .
cp SharpSnmpLib/bin/Release/*.snupkg .

echo "succeeded."
exit 0
