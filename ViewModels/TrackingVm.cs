using Application_Suivi_De_Temps.Models;
using System.ComponentModel.DataAnnotations;

namespace Application_Suivi_De_Temps.ViewModels;

public class TrackingVm
{
    public UsageSummary TodaySummary { get; set; } = new();
    public List<App> AvailableApps { get; set; } = new();
    public List<QuickAddOption> QuickAddOptions { get; set; } = new();
}

public class QuickAddOption
{
    public string Label { get; set; } = string.Empty;
    public int Minutes { get; set; }
    public string BadgeClass { get; set; } = "bg-secondary";
}

public class AddUsageInputModel
{
    [Required(ErrorMessage = "Sélectionnez une application")]
    public int AppId { get; set; }
    
    [Required(ErrorMessage = "Le temps est requis")]
    [Range(1, 720, ErrorMessage = "Le temps doit être entre 1 et 720 minutes (12h)")]
    public int Minutes { get; set; } = 15;
    
    public DateOnly? Date { get; set; }
}