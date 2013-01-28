set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
call %MSBuildDir%\msbuild SharpSnmpLib.sln /t:clean /p:Configuration=Release /p:OutputPath=..\bin\net35\
call %MSBuildDir%\msbuild SharpSnmpLib.sln /t:build /p:Configuration=Release /p:OutputPath=..\bin\net35\
call %MSBuildDir%\msbuild SharpSnmpLib.sln /t:clean /p:Configuration=Release /p:OutputPath=..\bin\net40\
call %MSBuildDir%\msbuild SharpSnmpLib.sln /t:build /p:Configuration=Release /p:TargetFrameworkVersion=v4.0 /p:OutputPath=..\bin\net40\
call %MSBuildDir%\msbuild SharpSnmpLib.cf35.sln /t:clean /p:Configuration=Release /p:OutputPath=..\bin\net35-cf\
call %MSBuildDir%\msbuild SharpSnmpLib.cf35.sln /t:build /p:Configuration=Release /p:OutputPath=..\bin\net35-cf\

md .nuget
cd .nuget
call nuget pack
cd ..
@IF %ERRORLEVEL% NEQ 0 PAUSE
