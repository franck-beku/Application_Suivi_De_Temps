namespace Application_Suivi_De_Temps.Models;

public class UsageSummary
{
    public DateOnly Date { get; set; }
    public int TotalMinutes { get; set; }
    public List<AppUsage> AppUsages { get; set; } = new();
    public List<CategoryUsage> CategoryUsages { get; set; } = new();
    
    // Helpers
    public string TotalDisplay
    {
        get
        {
            var hours = TotalMinutes / 60;
            var minutes = TotalMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
    
    public int TotalHours => TotalMinutes / 60;
    public int RemainingMinutes => TotalMinutes % 60;
}

public class AppUsage
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

public class CategoryUsage
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int Minutes { get; set; }
    public int Percentage { get; set; }
    public int AppCount { get; set; }
    
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