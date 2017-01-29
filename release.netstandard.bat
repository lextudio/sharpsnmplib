call dotnet restore SharpSnmpLib.NetStandard.sln
call dotnet clean SharpSnmpLib.NetStandard.sln /p:Configuration=Release /p:OutputPath=..\bin\ 
call dotnet build SharpSnmpLib.NetStandard.sln /p:Configuration=Release /p:OutputPath=..\bin\ 
