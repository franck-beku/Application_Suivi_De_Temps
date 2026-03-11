using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Models;

public class Tip
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public TipCategory Category { get; set; }
    public string IconClass { get; set; } = "bi-lightbulb";
    public bool IsActive { get; set; } = true;
    
    // Helpers
    public string CategoryDisplay
    {
        get
        {
            return Category switch
            {
                TipCategory.Health => "Santé",
                TipCategory.Productivity => "Productivité",
                TipCategory.WellBeing => "Bien-être",
                TipCategory.Habits => "Bonnes habitudes",
                TipCategory.Technology => "Technologie",
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
                TipCategory.Health => "#28a745",      // Vert
                TipCategory.Productivity => "#007bff", // Bleu
                TipCategory.WellBeing => "#6f42c1",    // Violet
                TipCategory.Habits => "#fd7e14",       // Orange
                TipCategory.Technology => "#17a2b8",   // Cyan
                _ => "#6c757d"                         // Gris
            };
        }
    }
}