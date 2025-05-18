# CONFIGURATION
$oldSolution = "sHoFSimpleJSONReader.sln"
$newSolution = "sHoFWeb.sln"

$oldProjectFolder = "HoFSimpleJSONReader"
$newProjectFolder = "HoFWeb"

$oldProjectFile = "$oldProjectFolder.csproj"
$newProjectFile = "$newProjectFolder.csproj"

$oldNamespace = "HoFSimpleJSONReader"
$newNamespace = "HoFWeb"

Write-Host "=== RENAMING STARTED ==="

# 1. Rename solution file
if (Test-Path $oldSolution) {
    Rename-Item $oldSolution $newSolution
    Write-Host "Renamed solution file to $newSolution"
} else {
    Write-Warning "Solution file '$oldSolution' not found."
}

# 2. Rename project file
if (Test-Path "$oldProjectFolder\$oldProjectFile") {
    Rename-Item "$oldProjectFolder\$oldProjectFile" $newProjectFile
    Write-Host "Renamed project file to $newProjectFile"
} else {
    Write-Warning "Project file '$oldProjectFile' not found in folder '$oldProjectFolder'."
}

# 3. Rename project folder
if (Test-Path $oldProjectFolder) {
    Rename-Item $oldProjectFolder $newProjectFolder
    Write-Host "Renamed project folder to $newProjectFolder"
} else {
    Write-Warning "Project folder '$oldProjectFolder' not found."
}

# 4. Update .sln references
if (Test-Path $newSolution) {
    (Get-Content $newSolution) -replace $oldProjectFile, $newProjectFile |
        ForEach-Object { $_ -replace $oldProjectFolder, $newProjectFolder } |
        Set-Content $newSolution
    Write-Host "Updated solution file references."
}

# 5. Update .csproj internals
$csprojPath = "$newProjectFolder\$newProjectFile"
if (Test-Path $csprojPath) {
    (Get-Content $csprojPath) -replace $oldNamespace, $newNamespace |
        Set-Content $csprojPath
    Write-Host "Updated project file internals."
}

# 6. Replace namespaces in all code files
$codeFiles = Get-ChildItem -Path $newProjectFolder -Recurse -Include *.cs,*.cshtml,*.razor,*.cshtml.cs

foreach ($file in $codeFiles) {
    (Get-Content $file.FullName) -replace $oldNamespace, $newNamespace |
        Set-Content $file.FullName
}
Write-Host "Replaced namespaces in code files."

# 7. Optional: update launchSettings.json
$launchSettings = "$newProjectFolder\Properties\launchSettings.json"
if (Test-Path $launchSettings) {
    (Get-Content $launchSettings) -replace $oldNamespace, $newNamespace |
        Set-Content $launchSettings
    Write-Host "Updated launchSettings.json."
}

Write-Host "=== RENAMING COMPLETE ==="
