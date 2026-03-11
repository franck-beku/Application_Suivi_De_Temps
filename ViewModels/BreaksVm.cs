using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application_Suivi_De_Temps.ViewModels;

public class BreaksVm
{
    public List<BreakActivity> AllActivities { get; set; } = new();
    public List<BreakSession> TodaySessions { get; set; } = new();
    public BreakStats Stats { get; set; } = new();
    
    public Dictionary<BreakCategory, List<BreakActivity>> ActivitiesByCategory
    {
        get
        {
            return AllActivities
                .GroupBy(a => a.Category)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}

public class RecordBreakInputModel
{
    [Required(ErrorMessage = "Sélectionnez une activité")]
    public int ActivityId { get; set; }
    
    [Required(ErrorMessage = "La durée est requise")]
    [Range(1, 120, ErrorMessage = "La durée doit être entre 1 et 120 minutes")]
    public int DurationMinutes { get; set; } = 10;
    
    [MaxLength(500, ErrorMessage = "Les notes ne peuvent pas dépasser 500 caractères")]
    public string? Notes { get; set; }
}