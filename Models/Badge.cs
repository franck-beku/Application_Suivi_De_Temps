using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Models;

public class Badge
{
    public int Id { get; set; }
    
    // Détails
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public BadgeType Type { get; set; }
    public string IconClass { get; set; } = "bi-award";
    public string Color { get; set; } = "#ffc107";
    
    // Critères de déblocage
    public int RequiredValue { get; set; }
    public string UnlockCriteria { get; set; } = string.Empty;
    
    // Navigation
    public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
    
    // Helpers
    public string TypeDisplay
    {
        get
        {
            return Type switch
            {
                BadgeType.TimeManagement => "Gestion du temps",
                BadgeType.BreakMaster => "Maître des pauses",
                BadgeType.Consistency => "Régularité",
                BadgeType.Disconnect => "Déconnexion",
                BadgeType.Milestone => "Jalon",
                BadgeType.Special => "Spécial",
                _ => "Autre"
            };
        }
    }
}