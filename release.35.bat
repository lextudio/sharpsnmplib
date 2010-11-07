set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v3.5
call %MSBuildDir%\msbuild sharpsnmplib.vs2008.sln /p:Configuration=Release
@IF %ERRORLEVEL% NEQ 0 PAUSE
pause