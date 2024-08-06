if ($env:CI -eq "true") {
    exit 0
}

$signtool = Get-ChildItem -Path "${env:ProgramFiles(x86)}\Windows Kits" -Recurse -Filter "signtool.exe" | Select-Object -First 1 -ExpandProperty FullName
Write-host "Signtool path: $signtool"
if (Test-Path $signtool) {
    New-Item -ItemType Directory -Path ".\SharpSnmpLib\bin\Release" -Force | Out-Null
    Set-Location -Path ".\SharpSnmpLib\bin\Release"
    Get-ChildItem -Recurse -Include *.exe, *.dll | ForEach-Object {
        & $signtool verify /pa /q $_.FullName
        if ($LASTEXITCODE -ne 0) {
            Read-Host "Press Enter to continue..."
        }
    }
    Set-Location -Path "..\..\.."
    New-Item -ItemType Directory -Path ".\SharpSnmpLib.BouncyCastle\bin\Release" -Force | Out-Null
    Set-Location -Path ".\SharpSnmpLib.BouncyCastle\bin\Release"
    Get-ChildItem -Recurse -Include *.exe, *.dll | ForEach-Object {
        & $signtool verify /pa /q $_.FullName
        if ($LASTEXITCODE -ne 0) {
            Read-Host "Press Enter to continue..."
        }
    }
    Set-Location -Path "..\..\.."
}

Exit 0
