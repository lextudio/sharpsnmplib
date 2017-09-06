#!/bin/bash

set -e

dotnet build SharpSnmpLib.NetStandard.macOS.sln
cd Tests
cd CSharpCore
dotnet test