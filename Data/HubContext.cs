using Microsoft.EntityFrameworkCore;
using PingPulse.Models;

namespace PingPulse.Data;

public class HubContext : DbContext
{
    public HubContext(DbContextOptions<HubContext> options) : base(options) {}

    public DbSet<WebhookPayload> Notifications => Set<WebhookPayload>();
}
