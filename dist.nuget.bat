call release.bat
md .nuget
cd .nuget
call nuget pack
cd ..
@IF %ERRORLEVEL% NEQ 0 PAUSE
