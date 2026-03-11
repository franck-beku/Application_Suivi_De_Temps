namespace Application_Suivi_De_Temps.Models;

public class BreakSession
{
    public int Id { get; set; }
    
    // FK Client
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    
    // FK BreakActivity
    public int BreakActivityId { get; set; }
    public BreakActivity? BreakActivity { get; set; }
    
    // Détails de la session
    public int DurationMinutes { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }
    
    // Helpers
    public string DurationDisplay
    {
        get
        {
            if (DurationMinutes < 60)
                return $"{DurationMinutes} min";
            
            var hours = DurationMinutes / 60;
            var minutes = DurationMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
    
    public DateOnly SessionDate => DateOnly.FromDateTime(StartedAt);
}