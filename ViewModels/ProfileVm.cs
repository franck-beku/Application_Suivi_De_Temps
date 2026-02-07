using System.ComponentModel.DataAnnotations;

namespace Application_Suivi_De_Temps.ViewModels;

public class ProfileVm
{
    [Required(ErrorMessage = "Le nom d'affichage est requis")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
    [Display(Name = "Nom d'affichage")]
    public string DisplayName { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'objectif quotidien est requis")]
    [Range(30, 720, ErrorMessage = "L'objectif doit être entre 30 minutes (0.5h) et 720 minutes (12h)")]
    [Display(Name = "Objectif quotidien (minutes)")]
    public int DefaultDailyGoalMinutes { get; set; } = 180;

    [Display(Name = "Activer les notifications")]
    public bool EnableNotifications { get; set; } = true;

    [Display(Name = "Activer les rappels de pause")]
    public bool EnableBreakReminders { get; set; } = true;

    [Display(Name = "Activer le rapport hebdomadaire")]
    public bool EnableWeeklyReport { get; set; } = true;

    [Display(Name = "Heure de début de déconnexion")]
    public TimeOnly? DisconnectStartTime { get; set; }

    [Display(Name = "Heure de fin de déconnexion")]
    public TimeOnly? DisconnectEndTime { get; set; }

    // Helper pour afficher l'objectif en heures et minutes
    public string DefaultDailyGoalDisplay
    {
        get
        {
            var hours = DefaultDailyGoalMinutes / 60;
            var minutes = DefaultDailyGoalMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
}