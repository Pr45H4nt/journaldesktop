# Run Journal App with detailed logging
Write-Host "Looking for JournalApp.exe..." -ForegroundColor Cyan

$exePath = Get-ChildItem -Path ".\JournalApp\bin\Debug\net8.0-windows10.0.19041.0" -Recurse -Filter "JournalApp.exe" -ErrorAction SilentlyContinue | Select-Object -First 1

if ($exePath) {
    Write-Host "Found: $($exePath.FullName)" -ForegroundColor Green
    Write-Host ""
    Write-Host "Launching app..." -ForegroundColor Yellow
    Write-Host ""

    # Run the executable and capture any output
    & $exePath.FullName

    Write-Host ""
    Write-Host "Exit Code: $LASTEXITCODE" -ForegroundColor $(if ($LASTEXITCODE -eq 0) { "Green" } else { "Red" })
} else {
    Write-Host "JournalApp.exe not found. Building first..." -ForegroundColor Yellow
    Set-Location .\JournalApp
    dotnet build -f net8.0-windows10.0.19041.0

    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "Build successful. Looking for exe again..." -ForegroundColor Green
        $exePath = Get-ChildItem -Path ".\bin\Debug\net8.0-windows10.0.19041.0" -Recurse -Filter "JournalApp.exe" -ErrorAction SilentlyContinue | Select-Object -First 1

        if ($exePath) {
            Write-Host "Found: $($exePath.FullName)" -ForegroundColor Green
            Write-Host "Launching app..." -ForegroundColor Yellow
            & $exePath.FullName
            Write-Host ""
            Write-Host "Exit Code: $LASTEXITCODE" -ForegroundColor $(if ($LASTEXITCODE -eq 0) { "Green" } else { "Red" })
        } else {
            Write-Host "Still can't find JournalApp.exe" -ForegroundColor Red
            Write-Host "Contents of bin\Debug\net8.0-windows10.0.19041.0:" -ForegroundColor Yellow
            Get-ChildItem -Path ".\bin\Debug\net8.0-windows10.0.19041.0" -Recurse | Select-Object FullName
        }
    }
}
