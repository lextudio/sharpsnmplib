if ($env:CI -eq "true") {
    exit 0
}

$nuget = ".\nuget.exe"

if (!(Test-Path $nuget))
{
    Invoke-WebRequest https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile nuget.exe
}

& $nuget update /self | Write-Debug

& dotnet nuget sign *.nupkg --certificate-subject-name "Yang Li" --timestamper http://timestamp.digicert.com
& dotnet nuget verify --all *.nupkg | Write-Debug
if ($LASTEXITCODE -ne 0)
{
    Write-Host "NuGet package is not signed. Exit."
    exit $LASTEXITCODE
}

Write-Host "Verification finished."
