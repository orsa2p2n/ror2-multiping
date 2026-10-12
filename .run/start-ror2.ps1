
# Copy DLL to plugins
$plugins = Join-Path $env:ROR2_DEVPROFILEDIR 'BepInEx\plugins\'

Copy-Item "bin\Debug\netstandard2.1\multiping.*" $plugins -Include *.dll,*.pdb -Force

# Start game with dev profile
$gameExe = Join-Path $env:ROR2_GAMEDIR 'Risk of Rain 2.exe'
$preloader = Join-Path $env:ROR2_DEVPROFILEDIR 'BepInEx\core\BepInEx.Preloader.dll'

Start-Process -FilePath $gameExe -WorkingDirectory $env:ROR2_GAMEDIR -ArgumentList @(
    '--doorstop-enabled', 'true',
    '--doorstop-target-assembly', "`"$preloader`"",
    '--doorstop-mono-debug-enabled', 'true',
    '--doorstop-mono-debug-address', '127.0.0.1:10000',
    '--doorstop-mono-debug-suspend', 'true'
)

# Wait for listener
$tries = 0
while (-not (Get-NetTCPConnection -LocalPort 10000 -State Listen -ErrorAction SilentlyContinue) -and $tries -lt 120) {
    Start-Sleep -Milliseconds 500
    $tries++
}
