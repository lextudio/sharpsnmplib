call build.release.bat
copy SharpSnmpLib\bin\Release\*.nupkg .
copy SharpSnmpLib.BouncyCastle\bin\Release\*.nupkg .
@IF %ERRORLEVEL% NEQ 0 PAUSE
