..\nuget.exe update /self
for %%f in (.\*.nupkg) do ..\nuget.exe push %%f -Source https://www.nuget.org/api/v2/package
