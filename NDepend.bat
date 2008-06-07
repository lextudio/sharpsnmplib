set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v3.5
call %MSBuildDir%\msbuild Build.proj /t:NDepend
@IF %ERRORLEVEL% NEQ 0 PAUSE