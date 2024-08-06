if ($env:CI -eq "true") {
    exit 0
}

$nuget = Join-Path $PSScriptRoot "nuget.exe"
if (!(Test-Path $nuget))
{
    Invoke-WebRequest https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile nuget.exe
}

Write-Host "Sign NuGet packages."
& $nuget sign *.nupkg -CertificateSubjectName "Yang Li" -Timestamper http://timestamp.digicert.com | Write-Debug
& $nuget verify -All *.nupkg | Write-Debug
if ($LASTEXITCODE -ne 0)
{
    Write-Host "NuGet package is not signed. Exit."
    exit $LASTEXITCODE
}

Write-Host "Verification finished."
