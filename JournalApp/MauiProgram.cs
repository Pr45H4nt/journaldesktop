using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using JournalApp.Data;
using JournalApp.Services;
using QuestPDF.Infrastructure;

namespace JournalApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // Set QuestPDF license early
        QuestPDF.Settings.License = LicenseType.Community;

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "journal.db");

        builder.Services.AddDbContext<JournalDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        builder.Services.AddScoped<JournalService>();
        builder.Services.AddScoped<TagService>();
        builder.Services.AddScoped<StreakService>();
        builder.Services.AddScoped<AnalyticsService>();
        builder.Services.AddScoped<SecurityService>();
        builder.Services.AddScoped<ThemeService>();
        builder.Services.AddScoped<ExportService>();

        var app = builder.Build();

        try
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<JournalDbContext>();
                db.Database.EnsureCreated();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
        }

        return app;
    }
}
