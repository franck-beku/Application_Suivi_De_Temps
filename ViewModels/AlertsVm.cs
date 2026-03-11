using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.ViewModels;

public class AlertsVm
{
    public List<AlertLog> RecentAlerts { get; set; } = new();
    public List<AlertLog> UnreadAlerts { get; set; } = new();
    
    public bool HasAlerts => RecentAlerts.Any();
    public bool HasUnreadAlerts => UnreadAlerts.Any();
    public int UnreadCount => UnreadAlerts.Count;
}