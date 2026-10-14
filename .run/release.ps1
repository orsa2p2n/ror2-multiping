echo "Building release..."
dotnet clean
dotnet build -c Release

echo "Clearing release folder..."

Get-ChildItem "release\*" -Exclude "manifest.json" | Remove-Item -Force

echo "Copying files..."
$releaseDir = '.\release\'

Copy-Item "bin\Release\netstandard2.1\**.dll" $releaseDir -Force
Copy-Item "icon\icon.png" $releaseDir -Force
Copy-Item "README.md" $releaseDir -Force

Remove-Item -Recurse -Force "bin\Release\"

echo "Creating zip..."

$manifest = Get-Content "release\manifest.json" -Raw | ConvertFrom-Json
$zipName = "$($manifest.name)-$($manifest.version_number).zip"

Get-ChildItem "release\*" -Exclude *.zip | Compress-Archive -DestinationPath "release\$zipName" -Force

echo "Release build complete."
