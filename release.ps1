[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [string] $Configuration = 'Release'
)

$msBuild = "msbuild"
try
{
    & $msBuild /version
    Write-Host "Likely on Linux/macOS."
    & dotnet restore
    & dotnet clean -c $Configuration
    & dotnet build -c $Configuration
}
catch
{
    Write-Host "MSBuild doesn't exist. Use VSSetup instead."

    $msbuild = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe -products * -nologo | select-object -first 1
    if (![System.IO.File]::Exists($msBuild))
    {
        Write-Host "MSBuild doesn't exist. Exit."
        exit 1
    }

    Write-Host "MSBuild found. Compile the projects."

    & $msBuild /m /p:Configuration=$Configuration /t:restore
    & $msBuild /m /p:Configuration=$Configuration /t:clean
    & $msBuild /m /p:Configuration=$Configuration
}

if ($LASTEXITCODE -ne 0)
{
    Write-Host "Compilation failed. Exit."
    exit $LASTEXITCODE
}

Write-Host "Compilation finished."
