param(
    [switch]$DualInstance
)

# Build
echo "Building debug..."
dotnet clean
dotnet build -c Debug

# Copy DLL to plugins
echo "Copying DLL..."
$plugins = Join-Path $env:ROR2_DEVPROFILEDIR 'BepInEx\plugins\'

Copy-Item "bin\Debug\netstandard2.1\**" $plugins -Include *.dll,*.pdb -Force

echo "DLL Copied."

# Start game with dev profile
echo "Starting game and waiting for listener..."
$gameExe = Join-Path $env:ROR2_GAMEDIR 'Risk of Rain 2.exe'
$preloader = Join-Path $env:ROR2_DEVPROFILEDIR 'BepInEx\core\BepInEx.Preloader.dll'

Start-Process -FilePath $gameExe -WorkingDirectory $env:ROR2_GAMEDIR -ArgumentList @(
    '--doorstop-enabled', 'true',
    '--doorstop-target-assembly', "`"$preloader`"",
    '--doorstop-mono-debug-enabled', 'true',
    '--doorstop-mono-debug-address', '127.0.0.1:10000',
    '--doorstop-mono-debug-suspend', 'true'
)

if ($DualInstance) {
    echo "Starting second game instance..."
    Start-Process -FilePath $gameExe -WorkingDirectory $env:ROR2_GAMEDIR
}

# Wait for listener
$tries = 0
while (-not (Get-NetTCPConnection -LocalPort 10000 -State Listen -ErrorAction SilentlyContinue) -and $tries -lt 120) {
    Start-Sleep -Milliseconds 500
    $tries++
}

echo "Listener connected."
