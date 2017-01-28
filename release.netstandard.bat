call dotnet restore SharpSnmpLib.NetStandard.sln
call dotnet build SharpSnmpLib.NetStandard.sln /p:Configuration=Debug /p:OutputPath=..\bin\ 
