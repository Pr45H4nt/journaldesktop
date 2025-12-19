# Build Instructions for Windows

## Prerequisites

1. **Install .NET 8.0 SDK**
   - Download: https://dotnet.microsoft.com/download/dotnet/8.0
   - Install and restart your terminal

2. **Install .NET MAUI Workload**
   - Open PowerShell or Command Prompt **as Administrator**
   - Run:
     ```
     dotnet workload install maui
     ```
   - Wait for installation to complete (may take several minutes)

## Building the Application

1. **Open PowerShell or Command Prompt**

2. **Navigate to the project folder**:
   ```
   cd path\to\journal_desktopapp\JournalApp
   ```

3. **Restore NuGet packages**:
   ```
   dotnet restore
   ```

4. **Build the project**:
   ```
   dotnet build -f net8.0-windows10.0.19041.0
   ```

5. **Run the application**:
   ```
   dotnet run -f net8.0-windows10.0.19041.0
   ```

## Alternative: Using Visual Studio

1. Install Visual Studio 2022 with .NET MAUI workload
2. Open `JournalApp.sln`
3. Select "Windows Machine" as the target
4. Press F5 to run

## Troubleshooting

### Error: "Workload 'maui' not found"
**Solution**: Run PowerShell/CMD as Administrator and install MAUI workload:
```
dotnet workload install maui
```

### Error: SDK not found
**Solution**: Verify .NET 8.0 SDK is installed:
```
dotnet --version
```
Should show 8.0.x

### Build errors
**Solution**: Clean and rebuild:
```
dotnet clean
dotnet restore
dotnet build -f net8.0-windows10.0.19041.0
```

## Publishing (Optional)

To create a standalone executable:

```
dotnet publish -f net8.0-windows10.0.19041.0 -c Release -p:PublishSingleFile=true --self-contained
```

The executable will be in: `bin\Release\net8.0-windows10.0.19041.0\win-x64\publish\`

## Notes

- This project is Windows-only (Android/iOS support removed for simplicity)
- Database will be created automatically in your AppData folder on first run
- No internet connection required
