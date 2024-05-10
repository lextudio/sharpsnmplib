$file = Join-Path $PSScriptRoot "cert.txt"
if (-not (Test-Path $file))
{
    Write-Host "No certificate specified. Exit."
    exit 0
}

$cert = Get-Content -Path $file -TotalCount 1
$foundCert = Test-Certificate -Cert $cert -User
if(!$foundCert)
{
    Write-Host "Certificate doesn't exist. Exit."
    exit 0
}

$signtool="C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64\signtool.exe"
if (Test-Path $signtool) {
    Write-Output "sign the assembly"
    & $signtool sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a $args[0]
    if ($LASTEXITCODE -ne 0)
    {
        Write-Host 'Assembly $args[0] is not signed. Exit.'
    }
}

exit 0
