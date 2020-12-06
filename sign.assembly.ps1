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
