using Microsoft.EntityFrameworkCore;
using PingPulse.Data;
using PingPulse.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// 1. Fetch connection string and validate it's not null/empty
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("CRITICAL ERROR: 'DefaultConnection' connection string was not found or is empty!");
}

// 2. Register DbContext
builder.Services.AddDbContext<HubContext>(options => 
    options.UseSqlServer(connectionString));

var app = builder.Build();

// 3. Securely check and initialize the database on startup (with built-in retry fallback)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HubContext>();
    
    int retryCount = 0;
    bool databaseReady = false;
    
    while (!databaseReady && retryCount < 6)
    {
        try
        {
            Console.WriteLine($"[STARTUP] Attempting to connect/initialize SQL Server database (Attempt {retryCount + 1}/6)...");
            db.Database.EnsureCreated();
            databaseReady = true;
            Console.WriteLine("[STARTUP] Database connection established and tables validated successfully.");
        }
        catch (Exception ex)
        {
            retryCount++;
            if (retryCount >= 6)
            {
                Console.WriteLine("[CRITICAL] Could not connect to SQL Server. Max retry attempts reached.");
                throw;
            }
            
            Console.WriteLine($"[WARNING] SQL Server not ready yet: {ex.Message}. Waiting 5 seconds before retrying...");
            Thread.Sleep(5000); // Give SQL Server container more time to fully spin up
        }
    }
}

// 4. Map Endpoint Modules
app.MapWebhookEndpoints();

app.Run();
