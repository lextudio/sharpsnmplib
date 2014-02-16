set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
call %MSBuildDir%\msbuild SharpSnmpLib.sln /t:clean /p:Configuration=Release /p:OutputPath=..\bin\net40\
call %MSBuildDir%\msbuild SharpSnmpLib.sln /t:build /p:Configuration=Release /p:TargetFrameworkVersion=v4.0 /p:OutputPath=..\bin\net40\
call %MSBuildDir%\msbuild SharpSnmpLib.iOS.sln /t:clean /p:Configuration=Release /p:Platform=iPhone /p:OutputPath=..\bin\Monotouch\
call %MSBuildDir%\msbuild SharpSnmpLib.iOS.sln /t:build /p:Configuration=Release /p:Platform=iPhone /p:OutputPath=..\bin\Monotouch\
call %MSBuildDir%\msbuild SharpSnmpLib.Android.sln /t:clean /p:Configuration=Release /p:OutputPath=..\bin\Monoandroid\
call %MSBuildDir%\msbuild SharpSnmpLib.Android.sln /t:build /p:Configuration=Release /p:OutputPath=..\bin\Monoandroid\
md .nuget
cd .nuget
call nuget pack
cd ..
@IF %ERRORLEVEL% NEQ 0 PAUSE
