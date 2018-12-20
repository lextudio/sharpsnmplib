del SharpSnmpLib\bin\Release\*.nupkg
del SharpSnmpLib.BouncyCastle\bin\Release\*.nupkg
del *.nupkg
call build.release.bat
call sign3.bat
copy SharpSnmpLib\bin\Release\*.nupkg .
copy SharpSnmpLib.Engine\bin\Release\*.nupkg .
copy SharpSnmpLib.BouncyCastle\bin\Release\*.nupkg .

powershell -file sign.ps1
@IF %ERRORLEVEL% NEQ 0 PAUSE
