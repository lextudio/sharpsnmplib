del SharpSnmpLib\bin\Release\*.nupkg
del *.nupkg
call build.release.bat
IF %ERRORLEVEL% NEQ 0 exit /b 1
call sign3.bat
IF %ERRORLEVEL% NEQ 0 exit /b 1
copy SharpSnmpLib\bin\Release\*.nupkg .

powershell -ExecutionPolicy Bypass -file sign.nuget.ps1
@IF %ERRORLEVEL% NEQ 0 exit /b 1
