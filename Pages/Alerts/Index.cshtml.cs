using Application_Suivi_De_Temps.Services.Alerts;
using Application_Suivi_De_Temps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Suivi_De_Temps.Pages.Alerts;

public class IndexModel : PageModel
{
    private readonly IAlertService _alertService;
    private const int DefaultClientId = 1;

    public IndexModel(IAlertService alertService)
    {
        _alertService = alertService;
    }

    public AlertsVm ViewModel { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostMarkAllAsReadAsync()
    {
        await _alertService.MarkAllAsReadAsync(DefaultClientId);
        TempData["SuccessMessage"] = "Toutes les alertes ont été marquées comme lues";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostClearAllAlertsAsync()
    {
        await _alertService.ClearAllAlertsAsync(DefaultClientId);
        TempData["SuccessMessage"] = "Toutes les alertes ont été supprimées";
        return RedirectToPage();
    }

    private async Task LoadDataAsync()
    {
        // Générer les alertes si nécessaire
        await _alertService.CheckAndGenerateAlertsAsync(DefaultClientId);

        ViewModel = new AlertsVm
        {
            RecentAlerts = await _alertService.GetRecentAlertsAsync(DefaultClientId, 20),
            UnreadAlerts = await _alertService.GetUnreadAlertsAsync(DefaultClientId)
        };
    }
}