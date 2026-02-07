using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Models;

public class CategoryThreshold
{
    public int Id { get; set; }
    
    // FK Client
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    
    // FK Category
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
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