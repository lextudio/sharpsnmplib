if ($env:CI -eq "true") {
    exit 0
}

$cert = Get-ChildItem -Path Cert:\CurrentUser\My -CodeSigningCert | Select-Object -First 1
if ($cert -eq $null) {
    Write-Host "No code signing certificate found in MY store. Exit."
    exit 1
}

$signtool = Get-ChildItem -Path "C:\Program Files (x86)\Windows Kits" -Recurse -Filter "signtool.exe" | Select-Object -First 1 -ExpandProperty FullName
Write-host "Signtool path: $signtool"
if (Test-Path $signtool) {
    Write-Output "sign the assembly"
    & $signtool sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a $args[0]
    if ($LASTEXITCODE -ne 0)
    {
        Write-Host 'Assembly $args[0] is not signed. Exit.'
    }
}

exit 0
