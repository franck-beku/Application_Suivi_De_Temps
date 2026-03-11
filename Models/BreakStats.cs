namespace Application_Suivi_De_Temps.Models;

public class BreakStats
{
    public int TodayTotalMinutes { get; set; }
    public int WeeklyTotalMinutes { get; set; }
    public int TotalSessions { get; set; }
    public string FavoriteActivity { get; set; } = string.Empty;
    
    public string TodayDisplay
    {
        get
        {
            if (TodayTotalMinutes == 0) return "0 min";
            if (TodayTotalMinutes < 60) return $"{TodayTotalMinutes} min";
            
            var hours = TodayTotalMinutes / 60;
            var minutes = TodayTotalMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
    
    public string WeeklyDisplay
    {
        get
        {
            if (WeeklyTotalMinutes == 0) return "0 min";
            if (WeeklyTotalMinutes < 60) return $"{WeeklyTotalMinutes} min";
            
            var hours = WeeklyTotalMinutes / 60;
            var minutes = WeeklyTotalMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
}