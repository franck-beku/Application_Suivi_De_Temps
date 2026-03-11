namespace Application_Suivi_De_Temps.Models.Enums;

public enum GoalType
{
    DailyUsageLimit = 1,        // Respecter limite temps quotidien
    WeeklyUsageLimit = 2,       // Respecter limite temps hebdomadaire
    DailyBreaks = 3,            // Prendre X pauses par jour
    WeeklyBreaks = 4,           // Prendre X pauses par semaine
    ConsecutiveDays = 5,        // X jours consécutifs sous l'objectif
    DisconnectStreak = 6        // X jours consécutifs mode déconnexion
}