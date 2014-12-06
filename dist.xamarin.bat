call dist.nuget.bat
copy bin\SharpSnmpLib.iOS.dll xamarin_support
copy bin\SharpSnmpLib.Android.dll xamarin_support
copy bin\SharpSnmpLib.Portable.dll xamarin_support
cd xamarin_support
call pack.bat
@IF %ERRORLEVEL% NEQ 0 PAUSE
