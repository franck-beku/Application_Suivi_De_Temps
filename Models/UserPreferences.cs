namespace Application_Suivi_De_Temps.Models;

public class UserPreferences
{
    public int Id { get; set; }
    
    // FK Client
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    
    // Préférences générales
    public int DefaultDailyGoalMinutes { get; set; } = 180; // 3h
    public bool EnableNotifications { get; set; } = true;
    public bool EnableBreakReminders { get; set; } = true;
    public bool EnableWeeklyReport { get; set; } = true;
    
    // Horaires de déconnexion (pour UC3)
    public TimeOnly? DisconnectStartTime { get; set; } // Ex: 22:00
    public TimeOnly? DisconnectEndTime { get; set; }   // Ex: 07:00
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}