if ($env:CI -eq "true") {
    exit 0
}

& dotnet nuget sign *.nupkg --certificate-subject-name "Yang Li" --timestamper http://timestamp.digicert.com
& dotnet nuget verify --all *.nupkg | Write-Debug
if ($LASTEXITCODE -ne 0)
{
    Write-Host "NuGet package is not signed. Exit."
    exit $LASTEXITCODE
}

Write-Host "Verification finished."
