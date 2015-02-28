export EnableNuGetPackageRestore=true
mono --runtime=v4.0.30319 .nuget/NuGet.exe restore SharpSnmpLib.sln
xbuild SharpSnmpLib.sln
