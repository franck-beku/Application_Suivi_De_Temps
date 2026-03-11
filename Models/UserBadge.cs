namespace Application_Suivi_De_Temps.Models;

public class UserBadge
{
    public int Id { get; set; }
    
    // FK Client
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    
    // FK Badge
    public int BadgeId { get; set; }
    public Badge? Badge { get; set; }
    
    // Déblocage
    public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;
    
    // Helpers
    public string UnlockedAtDisplay => UnlockedAt.ToLocalTime().ToString("dd/MM/yyyy");
}