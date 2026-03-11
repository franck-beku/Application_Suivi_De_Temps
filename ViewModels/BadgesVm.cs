using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.ViewModels;

public class BadgesVm
{
    public List<UserBadge> UnlockedBadges { get; set; } = new();
    public List<Badge> LockedBadges { get; set; } = new();
    public int UnlockedCount { get; set; }
    public int TotalCount { get; set; }
    public int ProgressPercentage { get; set; }
}