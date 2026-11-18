# Creates/updates a daily Windows scheduled task for unattended product sync.
# Run PowerShell as Administrator once.
param(
    [string]$TaskName = "NovaShop-ProductSync",
    [string]$Time = "06:30",
    [string]$Filter = ""
)

$ErrorActionPreference = "Stop"
$script = Join-Path $PSScriptRoot "Run-ProductSync.ps1"
if (-not (Test-Path $script)) { throw "Missing $script" }

$arg = "-NoProfile -ExecutionPolicy Bypass -File `"$script`""
if ($Filter) { $arg += " -Filter `"$Filter`"" }

$action = New-ScheduledTaskAction -Execute "powershell.exe" -Argument $arg
$trigger = New-ScheduledTaskTrigger -Daily -At $Time
$settings = New-ScheduledTaskSettingsSet -StartWhenAvailable -DontStopIfGoingOnBatteries -AllowStartIfOnBatteries
$principal = New-ScheduledTaskPrincipal -UserId $env:USERNAME -LogonType Interactive -RunLevel Highest

Register-ScheduledTask -TaskName $TaskName -Action $action -Trigger $trigger -Settings $settings -Principal $principal -Force | Out-Null
Write-Host "Scheduled task '$TaskName' installed to run daily at $Time."
Write-Host "Reports: $env:USERPROFILE\Documents\NovaShop\reports"
Write-Host "Manual test: powershell -NoProfile -ExecutionPolicy Bypass -File `"$script`""
