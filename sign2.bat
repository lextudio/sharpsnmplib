set signtool="C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64\signtool.exe"
if exist %signtool% (
    echo "sign the assembly"
    %signtool% sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a "%1"
    IF %ERRORLEVEL% NEQ 0 (
        echo %ERRORLEVEL%
    )
)
