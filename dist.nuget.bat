del SharpSnmpLib\bin\Release\*.nupkg
del SharpSnmpLib.BouncyCastle\bin\Release\*.nupkg
del *.nupkg
call build.release.bat
call sign3.bat
copy SharpSnmpLib\bin\Release\*.nupkg .
copy SharpSnmpLib.BouncyCastle\bin\Release\*.nupkg .
call sign.bat
@IF %ERRORLEVEL% NEQ 0 PAUSE
