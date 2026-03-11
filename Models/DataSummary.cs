namespace Application_Suivi_De_Temps.Models;

public class DataSummary
{
    // Usage
    public int TotalUsageEntries { get; set; }
    public int TotalUsageMinutes { get; set; }
    public DateTime? FirstUsageDate { get; set; }
    public DateTime? LastUsageDate { get; set; }
    
    // Pauses
    public int TotalBreakSessions { get; set; }
    public int TotalBreakMinutes { get; set; }
    
    // Seuils
    public int TotalAppThresholds { get; set; }
    public int TotalCategoryThresholds { get; set; }
    
    // Blocages
    public int TotalAppBlocks { get; set; }
    public int ActiveAppBlocks { get; set; }
    
    // Alertes
    public int TotalAlerts { get; set; }
    public int UnreadAlerts { get; set; }
    
    // Objectifs et badges
    public int TotalGoals { get; set; }
    public int CompletedGoals { get; set; }
    public int UnlockedBadges { get; set; }
    public int TotalBadges { get; set; }
    
    // Helpers
    public string TotalUsageDisplay
    {
        get
        {
            if (TotalUsageMinutes == 0) return "0 min";
            if (TotalUsageMinutes < 60) return $"{TotalUsageMinutes} min";
            
            var hours = TotalUsageMinutes / 60;
            var minutes = TotalUsageMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
    
    public string TotalBreakDisplay
    {
        get
        {
            if (TotalBreakMinutes == 0) return "0 min";
            if (TotalBreakMinutes < 60) return $"{TotalBreakMinutes} min";
            
            var hours = TotalBreakMinutes / 60;
            var minutes = TotalBreakMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
}