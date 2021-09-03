[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [string] $Configuration = 'Release'
)

$msBuild = "msbuild"
$onWindows = $false
try
{
    & $msBuild /version
    Write-Host "Likely on Linux/macOS."
}
catch
{
    Write-Host "MSBuild doesn't exist. Use VSSetup instead."

    Install-Module VSSetup -Scope CurrentUser -Force
    Update-Module VSSetup
    $instance = Get-VSSetupInstance -All -Prerelease | Select-VSSetupInstance -Latest
    $installDir = $instance.installationPath
    Write-Host "Found VS in " + $installDir
    $msBuild = $installDir + '\MSBuild\Current\Bin\MSBuild.exe'
    if (![System.IO.File]::Exists($msBuild))
    {
        $msBuild = $installDir + '\MSBuild\15.0\Bin\MSBuild.exe'
        if (![System.IO.File]::Exists($msBuild))
        {
            Write-Host "MSBuild doesn't exist. Exit."
            exit 1
        }
        else
        {
            Write-Host "Likely on Windows with VS2017."
        }
    }
    else
    {
        Write-Host "Likely on Windows with VS2019."
    }

    $onWindows = $true
}

Write-Host "MSBuild found. Compile the projects."

& $msBuild /m /p:Configuration=$Configuration /t:restore
& $msBuild /m /p:Configuration=$Configuration /t:clean
& $msBuild /m /p:Configuration=$Configuration
if ($LASTEXITCODE -ne 0)
{
    Write-Host "Compilation failed. Exit."
    exit $LASTEXITCODE
}

Write-Host "Compilation finished."
