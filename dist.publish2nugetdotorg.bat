nuget update /self
for %%f in (.\*.nupkg) do nuget push %%f -Source https://api.nuget.org/v3/index.json
for %%f in (.\*.snupkg) do nuget push %%f -Source https://api.nuget.org/v3/index.json