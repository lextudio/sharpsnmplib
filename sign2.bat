set signtool="C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64\signtool.exe"
if exist f:\sign.txt (%signtool% sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a "%1")