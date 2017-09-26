#!/bin/bash

set -e

export FrameworkPathOverride=$(dirname $(which mono))/../lib/mono/4.5/

cd Tests
cd CSharpCore
dotnet test --filter "FullyQualifiedName~Lextm.SharpSnmpLib.Integration"
dotnet test --filter "FullyQualifiedName~Lextm.SharpSnmpLib.Unit"
cd ..
cd ..