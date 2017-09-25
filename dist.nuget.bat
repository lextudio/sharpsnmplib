call build.release.bat
copy SharpSnmpLib\bin\Release\*.nupkg .
@IF %ERRORLEVEL% NEQ 0 PAUSE
