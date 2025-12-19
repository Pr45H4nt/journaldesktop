# Journal App - Windows Setup Guide

## Critical Fixes Applied

The following critical files were missing and have been added:

1. **Platforms/Windows/App.xaml** - Windows MAUI application entry point
2. **Platforms/Windows/App.xaml.cs** - Windows MAUI application code-behind
3. **Resources/Fonts/** directory - Font resources folder

## Prerequisites

Before running the app on Windows, ensure you have:

### 1. Install .NET 8.0 SDK
Download and install from: https://dotnet.microsoft.com/download/dotnet/8.0

Verify installation:
```powershell
dotnet --version
# Should show 8.0.x
```

### 2. Install .NET MAUI Workload
Open PowerShell as Administrator and run:
```powershell
dotnet workload install maui
```

Verify installation:
```powershell
dotnet workload list
# Should show "maui" in the installed workloads
```

### 3. Download OpenSans Font (Optional but Recommended)
1. Download OpenSans-Regular.ttf from: https://fonts.google.com/specimen/Open+Sans
2. Place the file in: `JournalApp/Resources/Fonts/OpenSans-Regular.ttf`

Note: The app will run without this font, but the UI will use default system fonts.

## Build and Run Instructions

### Method 1: Using PowerShell Script (Recommended)
```powershell
cd journal_desktopapp
.\BUILD_AND_RUN.ps1
```

### Method 2: Manual Build
```powershell
cd journal_desktopapp\JournalApp
dotnet clean
dotnet restore
dotnet build -f net8.0-windows10.0.19041.0
dotnet run -f net8.0-windows10.0.19041.0
```

## What Was Fixed

### Problem
The app was building successfully but not launching. The build output showed:
```
Build succeeded in 0.5s
```
But the application window never appeared.

### Root Causes
1. **Missing Windows Platform Files**: The `Platforms/Windows/App.xaml` and `App.xaml.cs` files were missing. These are essential for initializing the MAUI Windows application.
2. **Missing Font Directory**: The `Resources/Fonts/` directory didn't exist, which could cause resource loading issues.
3. **Font Reference Issue**: The .csproj was excluding a font file that didn't exist instead of conditionally including fonts.

### Solutions Applied
1. Created `Platforms/Windows/App.xaml` with proper MauiWinUIApplication configuration
2. Created `Platforms/Windows/App.xaml.cs` with the CreateMauiApp() override
3. Created `Resources/Fonts/` directory with placeholder
4. Updated .csproj to conditionally include font files only if they exist

## Troubleshooting

### Issue: "Build succeeded" but app doesn't start
**Solution**: This was the original issue. Ensure all files from the fix are present:
- Check `Platforms/Windows/App.xaml` exists
- Check `Platforms/Windows/App.xaml.cs` exists
- Verify git pulled all changes

### Issue: Font warnings during build
**Solution**: Download OpenSans-Regular.ttf and place it in `Resources/Fonts/`

### Issue: "MAUI workload not found"
**Solution**: Install MAUI workload as Administrator:
```powershell
dotnet workload install maui
```

### Issue: Build errors about Windows SDK
**Solution**: Ensure you're running on Windows 10 (version 1809 or later) or Windows 11

### Issue: App crashes on startup
**Solution**:
1. Delete `bin` and `obj` folders
2. Run `dotnet restore`
3. Rebuild the project

## Database Location

On first run, the app creates a SQLite database at:
```
%LOCALAPPDATA%\Packages\com.journalapp.desktop_<random>\LocalCache\Local\journal.db
```

## Running from Visual Studio (Optional)

If you prefer using Visual Studio:
1. Open `JournalApp.sln`
2. Set the build configuration to `Debug` or `Release`
3. Set the target framework to `net8.0-windows10.0.19041.0`
4. Press F5 to run

## Next Steps

After the app launches successfully:
1. Create your first journal entry
2. Explore the mood tracking features
3. Check out the analytics dashboard
4. Try the calendar and timeline views
5. Test the search and filter functionality
6. Export entries to PDF
7. Toggle between light and dark themes

## Support

If you encounter issues:
1. Check that all prerequisites are installed
2. Ensure you're on Windows 10 (1809+) or Windows 11
3. Delete `bin` and `obj` folders and rebuild
4. Check the [objectives.txt](objectives.txt) for feature requirements
5. Review the console output for detailed error messages

## Git Workflow

When making changes:
```bash
git add .
git commit -m "Your message"
git push
```

Then on Windows:
```bash
git pull
```

This ensures all files are synchronized between your Ubuntu development machine and Windows testing machine.
