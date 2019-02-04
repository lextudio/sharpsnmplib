f:\nuget.exe update /self
for %%f in (.\*.nupkg) do f:\nuget.exe push %%f -Source https://www.nuget.org/api/v2/package