# Runs NovaShop product sync unattended and captures exit code + latest report path.
param(
    [string]$Filter = "",
    [string]$ExePath = "",
    [int]$TimeoutMinutes = 90
)

$ErrorActionPreference = "Stop"
$root = Split-Path $PSScriptRoot -Parent
if (-not (Test-Path (Join-Path $root "BU50-API"))) {
    $root = "C:\Users\mahdi\source\Workspaces\NovaShopProductsUpdates"
}

if (-not $ExePath) {
    $ExePath = Join-Path $root "BU50-API\bin\Debug\BU50-API.exe"
}

if (-not (Test-Path $ExePath)) {
    throw "Executable not found: $ExePath. Build the project first."
}

$reportDir = Join-Path $env:USERPROFILE "Documents\NovaShop\reports"
New-Item -ItemType Directory -Force -Path $reportDir | Out-Null
$logPath = Join-Path $reportDir ("runner-{0:yyyyMMdd}.log" -f (Get-Date))

function Write-RunLog([string]$message) {
    $line = "{0:yyyy-MM-dd HH:mm:ss} | {1}" -f (Get-Date), $message
    Add-Content -Path $logPath -Value $line -Encoding UTF8
    Write-Host $line
}

Get-Process BU50-API -ErrorAction SilentlyContinue | ForEach-Object {
    Write-RunLog "Stopping previous BU50-API pid=$($_.Id)"
    Stop-Process -Id $_.Id -Force
}

$argsList = @("--auto")
if ($Filter) { $argsList += "--filter=$Filter" }

Write-RunLog "Starting: $ExePath $($argsList -join ' ')"
$proc = Start-Process -FilePath $ExePath -ArgumentList $argsList -WorkingDirectory (Split-Path $ExePath) -PassThru
$deadline = (Get-Date).AddMinutes($TimeoutMinutes)

while (-not $proc.HasExited) {
    if ((Get-Date) -gt $deadline) {
        Write-RunLog "TIMEOUT after $TimeoutMinutes minutes — killing process"
        Stop-Process -Id $proc.Id -Force
        exit 3
    }
    Start-Sleep -Seconds 15
}

$exitCode = $proc.ExitCode
$latest = Join-Path $reportDir "sync-report-latest.txt"
Write-RunLog "ExitCode=$exitCode"
if (Test-Path $latest) {
    Write-RunLog "Latest report: $latest"
    Get-Content $latest -Encoding UTF8 | ForEach-Object { Write-RunLog $_ }
}

exit $exitCode
