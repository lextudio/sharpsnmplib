set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v3.5
call %MSBuildDir%\msbuild Build.proj /t:debug
%ERRORLEVEL% NEQ 0 PAUSE