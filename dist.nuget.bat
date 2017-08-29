call build.release.bat
md .nuget
cd .nuget
del *.nupkg /Q
call nuget update /self
call nuget pack
copy ..\SharpSnmpLib\bin\Release\*.nupkg .
cd ..
@IF %ERRORLEVEL% NEQ 0 PAUSE
