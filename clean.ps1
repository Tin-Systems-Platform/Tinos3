$ErrorActionPreference = 'Stop'

$root = $PSScriptRoot

$buildDirectories = @(
    Get-ChildItem -LiteralPath $root -Directory -Recurse -Force |
        Where-Object { $_.Name -in @('bin', 'obj') } |
        Sort-Object { $_.FullName.Length } -Descending
)

foreach ($directory in $buildDirectories) {
    Write-Host "Removing $($directory.FullName)"
    Remove-Item -LiteralPath $directory.FullName -Recurse -Force
}

foreach ($outputDirectoryName in @('output-x64', 'output-arm64')) {
    $outputDirectory = Join-Path $root $outputDirectoryName

    if (Test-Path -LiteralPath $outputDirectory) {
        Write-Host "Removing $outputDirectory"
        Remove-Item -LiteralPath $outputDirectory -Recurse -Force
    }
}
