rmdir /S /Q bin
mkdir bin
set msBuildDir=C:\Program Files (x86)\MSBuild\12.0\Bin
call "%MSBuildDir%\msbuild" SharpSnmpLib.vs2013.sln /t:clean /p:Configuration=Release /p:OutputPath=..\bin\
call "%MSBuildDir%\msbuild" SharpSnmpLib.vs2013.sln /t:build /p:Configuration=Release /p:OutputPath=..\bin\
md .nuget
cd .nuget
call nuget pack
cd ..
@IF %ERRORLEVEL% NEQ 0 PAUSE
