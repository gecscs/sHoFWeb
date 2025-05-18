Write-Host "=== UPDATING NAMESPACES AND REFERENCES ==="

# Set root folder
$rootPath = "C:\Users\Guy\source\repos\sHoFSimpleJSONReader"  # Adjust if needed
Set-Location $rootPath

# Define replacements
$replacements = @{
    "sHoFSimpleJSONReader" = "sHoFWeb"
    "HoFSimpleJSONReader" = "HoFWeb"
}

# File types to include
$fileTypes = @("*.sln", "*.csproj", "*.csproj.user", "*.cs", "*.cshtml", "*.cshtml.cs", "*.razor", "*.razor.cs", "*.config", "*.json", "*.csproj.filters")

# Find and update each file
foreach ($fileType in $fileTypes) {
    $files = Get-ChildItem -Path $rootPath -Recurse -Include $fileType -File -ErrorAction SilentlyContinue

    foreach ($file in $files) {
        (Get-Content $file.FullName) | 
            ForEach-Object {
                $line = $_
                foreach ($old in $replacements.Keys) {
                    $line = $line -replace [regex]::Escape($old), $replacements[$old]
                }
                $line
            } | Set-Content $file.FullName

        Write-Host "Updated references in: $($file.FullName)"
    }
}

Write-Host "=== UPDATE COMPLETE ==="
