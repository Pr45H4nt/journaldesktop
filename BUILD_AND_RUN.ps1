# Journal App - Clean Build and Run Script
# Run this in PowerShell

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Journal App - Clean Build Process" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Navigate to project directory
Write-Host "[1/5] Navigating to project directory..." -ForegroundColor Yellow
Set-Location -Path "$PSScriptRoot\JournalApp"

# Step 2: Clean bin and obj folders
Write-Host "[2/5] Cleaning bin and obj folders..." -ForegroundColor Yellow
if (Test-Path "bin") {
    Remove-Item -Path "bin" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  ✓ Deleted bin folder" -ForegroundColor Green
}
if (Test-Path "obj") {
    Remove-Item -Path "obj" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  ✓ Deleted obj folder" -ForegroundColor Green
}

# Step 3: Restore NuGet packages
Write-Host "[3/5] Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✓ Restore successful" -ForegroundColor Green
} else {
    Write-Host "  ✗ Restore failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host ""

# Step 4: Build the project
Write-Host "[4/5] Building the project..." -ForegroundColor Yellow
Write-Host ""
dotnet build
if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "  ✓ Build successful!" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "  ✗ Build failed with exit code $LASTEXITCODE" -ForegroundColor Red
    Write-Host ""
    Write-Host "Check the error messages above for details." -ForegroundColor Yellow
    exit $LASTEXITCODE
}

Write-Host ""

# Step 5: Run the application
Write-Host "[5/5] Starting Journal App..." -ForegroundColor Yellow
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Application is starting..." -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

dotnet run
