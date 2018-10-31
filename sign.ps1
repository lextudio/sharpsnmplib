Remove-Item -Path .\*.nupkg
$nuget = "f:\nuget.exe"
& $nuget update /self | Write-Debug
& $nuget pack

Write-Host "Sign NuGet packages."
& $nuget sign *.nupkg -CertificateSubjectName "Yang Li" -Timestamper http://timestamp.digicert.com | Write-Debug
& $nuget verify -All *.nupkg | Write-Debug
if ($LASTEXITCODE -ne 0)
{
    Write-Host "NuGet package is not signed. Exit."
    exit $LASTEXITCODE
}

Write-Host "Verification finished."
