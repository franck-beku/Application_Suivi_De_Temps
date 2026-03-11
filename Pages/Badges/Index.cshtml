using Application_Suivi_De_Temps.Services.Badges;
using Application_Suivi_De_Temps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Suivi_De_Temps.Pages.Badges;

public class IndexModel : PageModel
{
    private readonly IBadgeService _badgeService;
    private const int DefaultClientId = 1;

    public IndexModel(IBadgeService badgeService)
    {
        _badgeService = badgeService;
    }

    public BadgesVm ViewModel { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDataAsync();
        return Page();
    }

    private async Task LoadDataAsync()
    {
        var unlockedBadges = await _badgeService.GetUnlockedBadgesAsync(DefaultClientId);
        var lockedBadges = await _badgeService.GetLockedBadgesAsync(DefaultClientId);
        var stats = await _badgeService.GetBadgeStatsAsync(DefaultClientId);

        ViewModel = new BadgesVm
        {
            UnlockedBadges = unlockedBadges,
            LockedBadges = lockedBadges,
            UnlockedCount = stats.unlocked,
            TotalCount = stats.total,
            ProgressPercentage = stats.percentage
        };
    }
}