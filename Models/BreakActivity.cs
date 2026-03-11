using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Models;

public class BreakActivity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public BreakCategory Category { get; set; }
    public int SuggestedDurationMinutes { get; set; } = 5;
    public string IconClass { get; set; } = "bi-circle";
    public bool IsActive { get; set; } = true;
    
    // Navigation
    public ICollection<BreakSession> BreakSessions { get; set; } = new List<BreakSession>();
    
    // Helpers
    public string CategoryDisplay
    {
        get
        {
            return Category switch
            {
                BreakCategory.Physical => "Physique",
                BreakCategory.Mental => "Mental",
                BreakCategory.Creative => "Créatif",
                BreakCategory.Social => "Social",
                BreakCategory.Relaxation => "Détente",
                BreakCategory.Productive => "Productif",
                _ => "Autre"
            };
        }
    }
    
    public string CategoryColor
    {
        get
        {
            return Category switch
            {
                BreakCategory.Physical => "#28a745",    // Vert
                BreakCategory.Mental => "#6f42c1",      // Violet
                BreakCategory.Creative => "#fd7e14",    // Orange
                BreakCategory.Social => "#20c997",      // Teal
                BreakCategory.Relaxation => "#17a2b8",  // Cyan
                BreakCategory.Productive => "#ffc107",  // Jaune
                _ => "#6c757d"                          // Gris
            };
        }
    }
}