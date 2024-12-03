# Usage: .\publish.ps1 -k nugetKey

param (
    [string]$key
)

$projects = Get-ChildItem -Recurse -Filter *.csproj | Where-Object { $_.Name -notmatch "\.Test\.csproj$" }
$deployTo = "https://api.nuget.org/v3/index.json"

if (!$key) {
    Write-Host "No API key provided, skipping publishing to ${deployTo}" -ForegroundColor Yellow
}

foreach ($project in $projects) {
    $projectPath = $project.FullName
    $projectName = [System.IO.Path]::GetFileNameWithoutExtension($projectPath)
    [xml]$csproj = Get-Content $projectPath

    $version = $csproj.Project.PropertyGroup.Version
    $version = "${version}".Trim()
    $packageFileName = "${projectName}.${version}.nupkg"
    $packageSource = Join-Path -Path $project.DirectoryName "bin" "Release" $packageFileName
    $packageDestination = Join-Path -Path $project.DirectoryName ".." ".build"
    $packageDestinationFile = Join-Path -Path $packageDestination $packageFileName

    Write-Host "Releasing ${projectName} ${version}" -ForegroundColor Green
    Write-Host "Building..."
    Invoke-Expression "dotnet build ${projectPath} -c Release" > $null 2>&1
    Write-Host "Packing..."
    Invoke-Expression "dotnet pack ${projectPath} -c Release" > $null 2>&1

    if (Test-Path -Path $packageDestinationFile) {
        Remove-Item -Path $packageDestinationFile
    }
    Move-Item -Path $packageSource -Destination $packageDestination

    if ($key) {
        Write-Host "Publishing..." -ForegroundColor Green
        Invoke-Expression "dotnet nuget push ${packageDestinationFile} --api-key ${key} --source ${deployTo}"
    }
}

Write-Host "Done" -ForegroundColor Green
