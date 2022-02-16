[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [string] $Path
)

Write-Host $Ppath

$winfx = "$path/Sdks/Microsoft.NET.Sdk.WindowsDesktop/targets/Microsoft.WinFX.props"
if (-not (Test-Path $winfx)) {
    Write-Host "Didn't find $winfx"
    $winfx2 = "$path/Sdks/Microsoft.NET.Sdk.WindowsDesktop/targets/Microsoft.WinFx.props"
    if (Test-Path $winfx2) {
        Write-Host "Found $winfx2. Copy it as $winfx"
        Copy-Item $winfx2 $winfx
    }
}
