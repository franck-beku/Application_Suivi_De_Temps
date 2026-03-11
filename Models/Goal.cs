using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Models;

public class Goal
{
    public int Id { get; set; }
    
    // FK Client
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    
    // Type et détails
    public GoalType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Cible et progression
    public int TargetValue { get; set; }
    public int CurrentValue { get; set; }
    
    // Période
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    // Statut
    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedAt { get; set; }
    
    // Métadonnées
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Helpers
    public int ProgressPercentage
    {
        get
        {
            if (TargetValue == 0) return 0;
            var percentage = (int)((CurrentValue / (double)TargetValue) * 100);
            return Math.Min(percentage, 100);
        }
    }
    
    public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate && !IsCompleted;
    
    public string ProgressDisplay => $"{CurrentValue} / {TargetValue}";
    
    public string ProgressBarColor
    {
        get
        {
            if (IsCompleted) return "bg-success";
            if (ProgressPercentage >= 75) return "bg-success";
            if (ProgressPercentage >= 50) return "bg-info";
            if (ProgressPercentage >= 25) return "bg-warning";
            return "bg-danger";
        }
    }
}