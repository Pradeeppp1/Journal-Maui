using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Journal.Data;
using Journal.Services;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Journal;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();

        // Configure database
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "journal.db");
        builder.Services.AddDbContext<JournalDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        // Register services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IJournalService, JournalService>();
        builder.Services.AddScoped<IProfileService, ProfileService>();
        builder.Services.AddScoped<IThemeService, ThemeService>();
        builder.Services.AddSingleton<ISecurityService, SecurityService>();
        builder.Services.AddScoped<IFileService, FileService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Initialize database
        var app = builder.Build();
        InitializeDatabase(app);

        return app;
    }

    private static void InitializeDatabase(MauiApp app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JournalDbContext>();
        
        try 
        {
            // Ensure any database at least exists
            dbContext.Database.EnsureCreated();

            // Explicitly create tables if they might be missing in an existing DB
            // This is safer for dev iterations where models change/add
            var sql = @"
                CREATE TABLE IF NOT EXISTS ""UserProfiles"" (
                    ""Id"" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    ""FullName"" TEXT NULL,
                    ""Email"" TEXT NULL,
                    ""Username"" TEXT NULL,
                    ""Bio"" TEXT NULL,
                    ""ProfilePictureUrl"" TEXT NULL,
                    ""IsDarkMode"" INTEGER NOT NULL,
                    ""IsCompactView"" INTEGER NOT NULL
                );";
            
            dbContext.Database.ExecuteSqlRaw(sql);

            // Dynamic Migration - Add missing columns if they don't exist
            try {
                dbContext.Database.ExecuteSqlRaw("ALTER TABLE UserProfiles ADD COLUMN Password TEXT DEFAULT '';");
            } catch { }

            try {
                dbContext.Database.ExecuteSqlRaw("ALTER TABLE JournalEntries ADD COLUMN SecondaryMoods TEXT DEFAULT '';");
            } catch { }

            try {
                dbContext.Database.ExecuteSqlRaw("ALTER TABLE JournalEntries ADD COLUMN UserId INTEGER DEFAULT 0;");
            } catch { }

            try {
                dbContext.Database.ExecuteSqlRaw("ALTER TABLE UserProfiles ADD COLUMN Pin TEXT DEFAULT '';");
            } catch { }

        }
        catch (Exception ex)
        {
            // Log error or handle appropriately
            System.Diagnostics.Debug.WriteLine($"Database initialization failed: {ex.Message}");
        }
    }
}