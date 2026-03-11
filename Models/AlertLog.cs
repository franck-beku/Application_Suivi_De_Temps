using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Models;

public class AlertLog
{
    public int Id { get; set; }
    
    // FK Client
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    
    // Type et sévérité
    public AlertType Type { get; set; }
    public AlertSeverity Severity { get; set; }
    
    // Détails
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    
    // Contexte (optionnel)
    public int? AppId { get; set; }
    public App? App { get; set; }
    
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    
    // Valeurs
    public int CurrentMinutes { get; set; }
    public int ThresholdMinutes { get; set; }
    public int ExcessMinutes { get; set; }
    
    // Métadonnées
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
    
    // Helpers
    public string CurrentDisplay
    {
        get
        {
            var hours = CurrentMinutes / 60;
            var minutes = CurrentMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
    
    public string ThresholdDisplay
    {
        get
        {
            var hours = ThresholdMinutes / 60;
            var minutes = ThresholdMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
    
    public string ExcessDisplay
    {
        get
        {
            var hours = ExcessMinutes / 60;
            var minutes = ExcessMinutes % 60;
            return minutes > 0 ? $"+{hours}h {minutes}m" : $"+{hours}h";
        }
    }
    
    public string SeverityBadgeClass
    {
        get
        {
            return Severity switch
            {
                AlertSeverity.Info => "bg-info",
                AlertSeverity.Warning => "bg-warning",
                AlertSeverity.Critical => "bg-danger",
                _ => "bg-secondary"
            };
        }
    }
    
    public string SeverityIcon
    {
        get
        {
            return Severity switch
            {
                AlertSeverity.Info => "bi-info-circle",
                AlertSeverity.Warning => "bi-exclamation-triangle",
                AlertSeverity.Critical => "bi-exclamation-octagon",
                _ => "bi-bell"
            };
        }
    }
}