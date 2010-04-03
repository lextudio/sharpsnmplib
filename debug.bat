set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
call %MSBuildDir%\msbuild Build.proj /t:debug
%ERRORLEVEL% NEQ 0 PAUSE