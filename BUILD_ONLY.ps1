# Journal App - Clean Build Only (No Run)
# Run this in PowerShell if you just want to test the build

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Journal App - Clean Build Test" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Navigate to project directory
Write-Host "[1/4] Navigating to project directory..." -ForegroundColor Yellow
Set-Location -Path "$PSScriptRoot\JournalApp"

# Step 2: Clean bin and obj folders
Write-Host "[2/4] Cleaning bin and obj folders..." -ForegroundColor Yellow
if (Test-Path "bin") {
    Remove-Item -Path "bin" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  ✓ Deleted bin folder" -ForegroundColor Green
}
if (Test-Path "obj") {
    Remove-Item -Path "obj" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  ✓ Deleted obj folder" -ForegroundColor Green
}

# Step 3: Restore NuGet packages
Write-Host "[3/4] Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✓ Restore successful" -ForegroundColor Green
} else {
    Write-Host "  ✗ Restore failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host ""

# Step 4: Build the project
Write-Host "[4/4] Building the project..." -ForegroundColor Yellow
Write-Host ""
dotnet build --verbosity detailed
if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "✓ BUILD SUCCESSFUL!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "To run the application, use:" -ForegroundColor Cyan
    Write-Host "  .\BUILD_AND_RUN.ps1" -ForegroundColor White
    Write-Host ""
    Write-Host "Or manually run:" -ForegroundColor Cyan
    Write-Host "  cd JournalApp" -ForegroundColor White
    Write-Host "  dotnet run" -ForegroundColor White
} else {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "✗ BUILD FAILED" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Check the error messages above." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Common solutions:" -ForegroundColor Yellow
    Write-Host "1. Make sure .NET 8.0 MAUI workload is installed:" -ForegroundColor White
    Write-Host "   dotnet workload install maui" -ForegroundColor Gray
    Write-Host ""
    Write-Host "2. If the error mentions PRI generation:" -ForegroundColor White
    Write-Host "   The Directory.Build.props should handle this" -ForegroundColor Gray
    Write-Host ""
    Write-Host "3. Try closing Visual Studio if it's open" -ForegroundColor White
    Write-Host "   and run this script again" -ForegroundColor Gray
    exit $LASTEXITCODE
}
