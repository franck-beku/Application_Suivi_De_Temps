namespace Application_Suivi_De_Temps.Models;

public class DashboardData
{
    // Temps d'écran aujourd'hui
    public int TodayMinutes { get; set; }
    public int DailyGoalMinutes { get; set; }
    public int ProgressPercentage { get; set; }
    public bool IsOverGoal { get; set; }
    public int MinutesOverGoal { get; set; }
    
    // Statistiques hebdomadaires
    public int WeeklyAverageMinutes { get; set; }
    public int WeeklyTotalMinutes { get; set; }
    public int WeeklyVariationPercentage { get; set; }
    public bool IsWeeklyIncreasing { get; set; }
    
    // Top applications
    public List<TopApp> TopApps { get; set; } = new();
    
    // État de déconnexion
    public DisconnectStatus? DisconnectStatus { get; set; }
    
    // Alertes (UC6) ← AJOUTÉ
    public List<AlertLog> UnreadAlerts { get; set; } = new();
    public bool HasUnreadAlerts => UnreadAlerts.Any();
    public int UnreadAlertsCount => UnreadAlerts.Count;
    
    // Suggestion de pause (UC8)
    public BreakActivity? SuggestedBreakActivity { get; set; }
    
    // Helpers pour affichage
    public string TodayDisplay
    {
        get
        {
            var hours = TodayMinutes / 60;
            var minutes = TodayMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
    
    public string DailyGoalDisplay
    {
        get
        {
            var hours = DailyGoalMinutes / 60;
            var minutes = DailyGoalMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
    
    public string WeeklyAverageDisplay
    {
        get
        {
            var hours = WeeklyAverageMinutes / 60;
            var minutes = WeeklyAverageMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
    
    public string WeeklyTotalDisplay
    {
        get
        {
            var hours = WeeklyTotalMinutes / 60;
            var minutes = WeeklyTotalMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
    
    public string MinutesOverGoalDisplay
    {
        get
        {
            if (MinutesOverGoal <= 0) return "0m";
            var hours = MinutesOverGoal / 60;
            var minutes = MinutesOverGoal % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
    
    public string ProgressBarColor
    {
        get
        {
            if (ProgressPercentage <= 75) return "bg-success";
            if (ProgressPercentage <= 100) return "bg-warning";
            return "bg-danger";
        }
    }
}

public class TopApp
{
    public int AppId { get; set; }
    public string AppName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int Minutes { get; set; }
    public int Percentage { get; set; }
    
    public string TimeDisplay
    {
        get
        {
            var hours = Minutes / 60;
            var minutes = Minutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
}