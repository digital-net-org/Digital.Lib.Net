param (
    [string]$ConnectionString
)

$MigrationName = Read-Host "Enter migration name"
$Project = "$PSScriptRoot/../Digital.Lib.Net.Entities"
dotnet ef migrations add $MigrationName --project $Project --context "DigitalContext" -- $ConnectionString
