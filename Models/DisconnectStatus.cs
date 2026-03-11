namespace Application_Suivi_De_Temps.Models;

public class DisconnectStatus
{
    public bool IsEnabled { get; set; }
    public bool IsInDisconnectPeriod { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string Message { get; set; } = string.Empty;
    
    // Helper pour affichage
    public string PeriodDisplay
    {
        get
        {
            if (StartTime.HasValue && EndTime.HasValue)
            {
                return $"{StartTime.Value:HH:mm} - {EndTime.Value:HH:mm}";
            }
            return "Non configuré";
        }
    }
}