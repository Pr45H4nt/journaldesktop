@echo off
echo ========================================
echo Journal App - Windows Build Script
echo ========================================
echo.

echo Restoring NuGet packages...
dotnet restore
if %errorlevel% neq 0 (
    echo ERROR: Failed to restore packages
    pause
    exit /b %errorlevel%
)
echo.

echo Building project...
dotnet build -f net8.0-windows10.0.19041.0
if %errorlevel% neq 0 (
    echo ERROR: Build failed
    pause
    exit /b %errorlevel%
)
echo.

echo ========================================
echo BUILD SUCCESSFUL!
echo ========================================
echo.
echo To run the app, execute: run.bat
echo Or run: dotnet run -f net8.0-windows10.0.19041.0
echo.
pause
