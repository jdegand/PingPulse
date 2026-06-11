namespace PingPulse.Models;

public class WebhookPayload
{
    public int Id { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
}
