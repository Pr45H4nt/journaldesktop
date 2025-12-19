# Journal App - Quick Start Guide

## Build the Application

### Windows PowerShell
```powershell
.\BUILD_ONLY.ps1
```

### Manual Build
```powershell
cd JournalApp
dotnet build
```

## Run the Application

### Windows PowerShell
```powershell
.\BUILD_AND_RUN.ps1
```

### Manual Run
```powershell
cd JournalApp
dotnet run
```

## What Was Fixed

The Windows App SDK PRI generation incompatibility with .NET SDK 9.0 has been resolved by:
1. ✅ Removing explicit Windows App SDK package reference
2. ✅ Disabling Windows App SDK bootstrapper
3. ✅ Overriding all PRI generation build targets
4. ✅ Multiple layers of PRI disabling properties

## Features

Your Journal App includes:
- ✅ Daily journaling (one entry per day)
- ✅ Mood tracking (15 moods, 3 selection slots)
- ✅ Tag system (31 pre-built + custom tags)
- ✅ Categories
- ✅ Calendar view
- ✅ Timeline view
- ✅ Search & filter
- ✅ Streak tracking
- ✅ Analytics dashboard
- ✅ Password protection
- ✅ PDF export
- ✅ Theme customization

## Database

SQLite database auto-created at:
```
%LOCALAPPDATA%\Packages\com.journalapp.desktop_[hash]\LocalState\journal.db
```

## Troubleshooting

### Build fails with "workload not found"
```powershell
dotnet workload install maui
```

### Need to delete bin/obj manually
```powershell
cd JournalApp
Remove-Item -Recurse -Force bin,obj -ErrorAction SilentlyContinue
```

### Still getting PRI errors
The Directory.Build.props should handle this, but if it persists, check that:
1. The file exists in the JournalApp folder
2. Visual Studio is closed
3. bin and obj folders are deleted

## Success Indicators

Build is successful when you see:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

Application launches when you see a desktop window titled "Journal App".
