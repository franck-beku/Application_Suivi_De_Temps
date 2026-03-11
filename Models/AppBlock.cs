namespace Application_Suivi_De_Temps.Models;

public class AppBlock
{
    public int Id { get; set; }
    
    // FK Client
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    
    // FK App
    public int AppId { get; set; }
    public App? App { get; set; }
    
    // Détails du blocage
    public DateTime BlockedUntil { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Helpers
    public bool IsExpired => DateTime.UtcNow >= BlockedUntil;
    
    public TimeSpan TimeRemaining
    {
        get
        {
            var remaining = BlockedUntil - DateTime.UtcNow;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }
    }
    
    public string TimeRemainingDisplay
    {
        get
        {
            var remaining = TimeRemaining;
            
            if (remaining == TimeSpan.Zero)
                return "Expiré";
            
            if (remaining.TotalDays >= 1)
                return $"{(int)remaining.TotalDays}j {remaining.Hours}h";
            
            if (remaining.TotalHours >= 1)
                return $"{remaining.Hours}h {remaining.Minutes}m";
            
            return $"{remaining.Minutes}m {remaining.Seconds}s";
        }
    }
    
    public string BlockedUntilDisplay => BlockedUntil.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
}