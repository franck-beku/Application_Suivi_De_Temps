using Application_Suivi_De_Temps.Services.DataManagement;
using Application_Suivi_De_Temps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Suivi_De_Temps.Pages.DataManagement;

public class IndexModel : PageModel
{
    private readonly IDataManagementService _dataService;
    private const int DefaultClientId = 1;

    public IndexModel(IDataManagementService dataService)
    {
        _dataService = dataService;
    }

    public DataManagementVm ViewModel { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteUsageAsync()
    {
        await _dataService.DeleteAllUsageDataAsync(DefaultClientId);
        TempData["SuccessMessage"] = "✅ Toutes les données d'usage ont été supprimées";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteBreaksAsync()
    {
        await _dataService.DeleteAllBreakSessionsAsync(DefaultClientId);
        TempData["SuccessMessage"] = "✅ Toutes les sessions de pause ont été supprimées";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteBlocksAsync()
    {
        await _dataService.DeleteAllAppBlocksAsync(DefaultClientId);
        TempData["SuccessMessage"] = "✅ Tous les blocages d'applications ont été supprimés";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAlertsAsync()
    {
        await _dataService.DeleteAllAlertsAsync(DefaultClientId);
        TempData["SuccessMessage"] = "✅ Toutes les alertes ont été supprimées";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteThresholdsAsync()
    {
        await _dataService.DeleteAllThresholdsAsync(DefaultClientId);
        TempData["SuccessMessage"] = "✅ Tous les seuils personnalisés ont été supprimés";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteGoalsAsync()
    {
        await _dataService.DeleteAllGoalsAsync(DefaultClientId);
        TempData["SuccessMessage"] = "✅ Tous les objectifs ont été supprimés";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostResetAllAsync()
    {
        await _dataService.ResetAllDataAsync(DefaultClientId);
        TempData["SuccessMessage"] = "✅ Toutes les données ont été réinitialisées";
        return RedirectToPage();
    }

    private async Task LoadDataAsync()
    {
        ViewModel = new DataManagementVm
        {
            Summary = await _dataService.GetDataSummaryAsync(DefaultClientId)
        };
    }
}