@echo off
echo ========================================
echo Journal App - Running...
echo ========================================
echo.

dotnet run -f net8.0-windows10.0.19041.0

if %errorlevel% neq 0 (
    echo.
    echo ERROR: Failed to run application
    echo Make sure you've built the project first using build.bat
    echo.
    pause
    exit /b %errorlevel%
)
