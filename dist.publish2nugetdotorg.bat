.\nuget.exe update /self
for %%f in (.\*.nupkg) do .\nuget.exe push %%f -Source https://api.nuget.org/v3/index.json
@REM Symbol package is pushed in this step as well.
