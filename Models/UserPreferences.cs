namespace Application_Suivi_De_Temps.Models;

public class UserPreferences
{
    public int Id { get; set; }
    
    // FK Client
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    
    // Préférences générales
    public int DefaultDailyGoalMinutes { get; set; } = 180; // 3h par défaut
    public bool EnableNotifications { get; set; } = true;
    public bool EnableBreakReminders { get; set; } = true;
    public bool EnableWeeklyReport { get; set; } = true;
    
    // Horaires de déconnexion (pour UC3 - déconnexion programmée)
    public TimeOnly? DisconnectStartTime { get; set; } // Ex: 22:00
    public TimeOnly? DisconnectEndTime { get; set; }   // Ex: 07:00
    public bool EnableDisconnectMode { get; set; } = false; // ← NOUVEAU pour UC3
    
    // Métadonnées
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}