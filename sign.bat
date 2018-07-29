set signtool="f:\nuget.exe"
%signtool% sign *.nupkg -CertificateSubjectName "Yang Li" -Timestamper http://timestamp.digicert.com
%signtool% verify -All *.nupkg
@IF %ERRORLEVEL% NEQ 0 PAUSE