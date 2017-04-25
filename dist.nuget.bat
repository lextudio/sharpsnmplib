call build.release.bat
md .nuget
cd .nuget
call nuget update /self
call nuget pack
cd ..
@IF %ERRORLEVEL% NEQ 0 PAUSE
