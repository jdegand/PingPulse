using PingPulse.Data;
using PingPulse.Models;

namespace PingPulse.Endpoints;

public static class WebhookEndpoints
{
    public static void MapWebhookEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/webhook", async (WebhookPayload payload, HubContext db) =>
        {
            db.Notifications.Add(payload);
            await db.SaveChangesAsync();
            
            Console.WriteLine($"[ALERT] Commit by {payload.Author}: {payload.Message}");
            return Results.Ok(new { status = "Success", message = "Payload stored in SQL Server" });
        });
    }
}
