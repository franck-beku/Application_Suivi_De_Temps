namespace Application_Suivi_De_Temps.Models.Enums;

public enum AlertType
{
    DailyGoalExceeded = 1,      // Objectif quotidien dépassé
    AppThresholdExceeded = 2,    // Seuil d'application dépassé
    CategoryThresholdExceeded = 3 // Seuil de catégorie dépassé
}