using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Models;

public class AppThreshold
{
    public int Id { get; set; }
    
    // FK Client
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    
    // FK App
    public int AppId { get; set; }
    public App? App { get; set; }
    
    // Seuil en minutes
    public int ThresholdMinutes { get; set; }
    
    // Type de seuil (quotidien/hebdomadaire)
    public ThresholdType Type { get; set; } = ThresholdType.Daily;
    
    // Actif ou non
    public bool IsActive { get; set; } = true;
    
    // Métadonnées
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}