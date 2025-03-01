param (
    [string]$ProjectPath,
    [string]$env
)

if (-not $env) {
    $env = "Development"
}

$AppSettingsPath = "${ProjectPath}/appsettings.${env}.json"

if (-Not (Test-Path $AppSettingsPath)) {
    Write-Host "Missing ${env} appsettings file: ${AppSettingsPath}"
    exit 1
}

$JsonContent = Get-Content -Raw $AppSettingsPath | ConvertFrom-Json
$ConnectionString = $JsonContent.ConnectionStrings.Default

if (-not $ConnectionString) {
    Write-Host "Missing value 'ConnectionStrings:Default' in $AppSettingsPath"
    exit 1
}

return $ConnectionString
