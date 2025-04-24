for %%f in (.\*.nupkg) do dotnet nuget push %%f --source https://api.nuget.org/v3/index.json -k %NUGET_API_KEY%
@REM Symbol package is pushed in this step as well.
