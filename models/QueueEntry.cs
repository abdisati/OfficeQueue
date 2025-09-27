namespace OfficeQueue.Models;

public class QueueEntry
{
    public int Id { get; set; }
    public long TelegramId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Service { get; set; } = string.Empty;
    public int QueueNumber { get; set; }
    public string Status { get; set; } = "waiting"; // waiting, served, canceled
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
