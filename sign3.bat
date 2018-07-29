set signtool="C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64\signtool.exe"
if exist f:\sign.txt (
mkdir .\SharpSnmpLib\bin\Release
cd .\SharpSnmpLib\bin\Release
for /r %%i in (*.exe *.dll) do (
%signtool% verify /pa /q "%%i"
@IF %ERRORLEVEL% NEQ 0 PAUSE
)
cd ..\..\..
mkdir .\SharpSnmpLib.BouncyCastle\bin\Release
cd .\SharpSnmpLib.BouncyCastle\bin\Release
for /r %%i in (*.exe *.dll) do (
%signtool% verify /pa /q "%%i"
@IF %ERRORLEVEL% NEQ 0 PAUSE
)
cd ..\..\..
)