# Journal App - Troubleshooting Guide

## Issue: App Builds But Doesn't Open

### Fixes Applied
1. Added error handling to database initialization
2. Set QuestPDF license early in MauiProgram
3. Created proper Windows platform App.xaml files

### Diagnostic Steps to Run on Windows

#### Step 1: Check if the process starts and crashes
```powershell
cd "C:\Users\ASUS\OneDrive - London Metropolitan University\Desktop\prashant\journaldesktop\JournalApp"

# Clean and rebuild
dotnet clean
dotnet build -f net8.0-windows10.0.19041.0

# Try running directly
dotnet run -f net8.0-windows10.0.19041.0
```

Watch for:
- Does a console window appear briefly and disappear?
- Does Task Manager show JournalApp.exe starting and stopping?
- Are there any error messages in the console?

#### Step 2: Run the executable directly
```powershell
# Navigate to the output directory
cd bin\Debug\net8.0-windows10.0.19041.0\win-x64

# Run the exe directly
.\JournalApp.exe
```

This might show error messages that `dotnet run` doesn't display.

#### Step 3: Check Windows Event Viewer
1. Press `Win + R`
2. Type `eventvwr.msc` and press Enter
3. Navigate to: Windows Logs → Application
4. Look for recent errors with source ".NET Runtime" or "Application Error"
5. Check the error details for clues

#### Step 4: Enable detailed logging
Create a file `appsettings.Development.json` in the JournalApp directory:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Debug",
      "Microsoft.AspNetCore": "Debug"
    }
  }
}
```

Then run with:
```powershell
$env:DOTNET_ENVIRONMENT="Development"
dotnet run -f net8.0-windows10.0.19041.0
```

#### Step 5: Check for missing dependencies
Run this PowerShell command to verify .NET MAUI workload:
```powershell
dotnet workload list
```

You should see `maui` in the installed workloads. If not:
```powershell
# Run as Administrator
dotnet workload install maui
```

#### Step 6: Verify Windows App SDK
The app requires Windows App SDK. Check if it's installed:
```powershell
Get-AppxPackage | Where-Object {$_.Name -like "*WindowsAppRuntime*"}
```

If missing, it should be installed automatically, but you can install manually:
```powershell
winget install Microsoft.WindowsAppSDK
```

### Common Issues and Solutions

#### Issue: Silent crash on startup
**Possible causes:**
- Missing Windows App SDK runtime
- Database path permission issues
- QuestPDF license not set (fixed)

**Solution:**
Try creating the app data directory manually:
```powershell
$appDataPath = "$env:LOCALAPPDATA\Packages\com.journalapp.desktop_*\LocalCache\Local"
New-Item -ItemType Directory -Path $appDataPath -Force
```

#### Issue: DLL not found errors
**Solution:**
```powershell
dotnet restore --force
dotnet build --no-restore
```

#### Issue: App starts but window doesn't appear
**Possible cause:** Windows display scaling or multi-monitor issues

**Solution:**
Try right-clicking the exe → Properties → Compatibility → Change high DPI settings → Check "Override high DPI scaling behavior"

### Debug Build with Console Output

To see console output, temporarily modify Program.cs to add a console:

```csharp
[STAThread]
static void Main(string[] args)
{
    // Attach console for debugging
    if (!AttachConsole(-1))
        AllocConsole();

    Console.WriteLine("Starting Journal App...");

    try
    {
        WinRT.ComWrappersSupport.InitializeComWrappers();
        Microsoft.UI.Xaml.Application.Start((p) =>
        {
            var context = new Microsoft.UI.Dispatching.DispatcherQueueSynchronizationContext(
                Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread());
            System.Threading.SynchronizationContext.SetSynchronizationContext(context);
            _ = new App();
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        Console.ReadLine();
    }
}

[System.Runtime.InteropServices.DllImport("kernel32.dll")]
static extern bool AttachConsole(int dwProcessId);

[System.Runtime.InteropServices.DllImport("kernel32.dll")]
static extern bool AllocConsole();
```

### Alternative: Check Output Type

The project is set to `<OutputType>Exe</OutputType>`. On Windows, this creates a console-less executable. For debugging, you could temporarily change it to see console output, but this should not be necessary with proper error handling.

### What to Report Back

When you test, please provide:
1. Any error messages from the console
2. Event Viewer error details (if any)
3. Does the process appear in Task Manager? For how long?
4. Output from `dotnet run --verbosity detailed`
5. Any crash dumps or error codes

### Expected Behavior

When working correctly:
1. `dotnet run` executes
2. A MAUI window appears with the Journal App UI
3. The home page shows "Welcome to Your Journal"
4. Database is created in `%LOCALAPPDATA%\Packages\com.journalapp.desktop_*\LocalCache\Local\journal.db`

### Emergency Fallback: Minimal Test

If nothing works, create a minimal test project:
```powershell
dotnet new maui -n TestMaui
cd TestMaui
dotnet build
dotnet run
```

If the test project works but JournalApp doesn't, the issue is with the code. If the test project also fails, it's an environment/setup issue.
