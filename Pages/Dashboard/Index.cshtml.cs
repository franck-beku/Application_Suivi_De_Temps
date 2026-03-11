using Application_Suivi_De_Temps.Services.Dashboard;
using Application_Suivi_De_Temps.Services.Alerts;
using Application_Suivi_De_Temps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Suivi_De_Temps.Pages.Dashboard;

public class IndexModel : PageModel
{
    private readonly IDashboardService _dashboardService;
    private readonly IAlertService _alertService; // ← AJOUTÉ pour UC6
    private const int DefaultClientId = 1;

    public IndexModel(IDashboardService dashboardService, IAlertService alertService) // ← AJOUTÉ alertService
    {
        _dashboardService = dashboardService;
        _alertService = alertService; // ← AJOUTÉ pour UC6
    }

    public DashboardVm ViewModel { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var data = await _dashboardService.GetDashboardDataAsync(DefaultClientId);
        
        ViewModel = new DashboardVm
        {
            Data = data
        };
        
        return Page();
    }

    // ← AJOUTÉ pour UC6 : Handler pour marquer les alertes comme lues
    public async Task<IActionResult> OnPostMarkAlertsAsReadAsync()
    {
        await _alertService.MarkAllAsReadAsync(DefaultClientId);
        TempData["SuccessMessage"] = "Toutes les alertes ont été marquées comme lues";
        return RedirectToPage();
    }
}