#!/bin/bash

set -e

sed -i -e 's/netstandard1.3;net452/netstandard1.3/' SharpSnmpLib/SharpSnmpLib.csproj

dotnet build SharpSnmpLib.NetStandard.macOS.sln
cd Tests
cd CSharpCore
dotnet test
cd ..
cd ..