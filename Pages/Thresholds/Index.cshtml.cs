using Application_Suivi_De_Temps.Services.Threshold;
using Application_Suivi_De_Temps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Suivi_De_Temps.Pages.Thresholds;

public class IndexModel : PageModel
{
    private readonly IThresholdService _thresholdService;
    private const int DefaultClientId = 1;

    public IndexModel(IThresholdService thresholdService)
    {
        _thresholdService = thresholdService;
    }

    public ThresholdVm Data { get; set; } = new();
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostSetCategoryThresholdAsync(int categoryId, int thresholdMinutes)
    {
        try
        {
            await _thresholdService.SetCategoryThresholdAsync(DefaultClientId, categoryId, thresholdMinutes);
            TempData["SuccessMessage"] = "Seuil de catégorie mis à jour avec succès !";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur : {ex.Message}";
            await LoadDataAsync();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostRemoveCategoryThresholdAsync(int categoryId)
    {
        try
        {
            await _thresholdService.RemoveCategoryThresholdAsync(DefaultClientId, categoryId);
            TempData["SuccessMessage"] = "Seuil de catégorie supprimé avec succès !";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur : {ex.Message}";
            await LoadDataAsync();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostSetAppThresholdAsync(int appId, int thresholdMinutes)
    {
        try
        {
            await _thresholdService.SetAppThresholdAsync(DefaultClientId, appId, thresholdMinutes);
            TempData["SuccessMessage"] = "Seuil d'application mis à jour avec succès !";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur : {ex.Message}";
            await LoadDataAsync();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostRemoveAppThresholdAsync(int appId)
    {
        try
        {
            await _thresholdService.RemoveAppThresholdAsync(DefaultClientId, appId);
            TempData["SuccessMessage"] = "Seuil d'application supprimé avec succès !";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur : {ex.Message}";
            await LoadDataAsync();
            return Page();
        }
    }

    private async Task LoadDataAsync()
    {
        var categoryThresholds = await _thresholdService.GetCategoryThresholdsAsync(DefaultClientId);
        var appThresholds = await _thresholdService.GetAppThresholdsAsync(DefaultClientId);

        Data = new ThresholdVm
        {
            CategoryThresholds = categoryThresholds.Select(t => new CategoryThresholdItem
            {
                CategoryId = t.CategoryId,
                CategoryName = t.Category?.Name ?? "",
                ThresholdMinutes = t.ThresholdMinutes,
                IsActive = t.IsActive
            }).ToList(),

            AppThresholds = appThresholds.Select(t => new AppThresholdItem
            {
                AppId = t.AppId,
                AppName = t.App?.Name ?? "",
                CategoryName = t.App?.Category?.Name ?? "",
                ThresholdMinutes = t.ThresholdMinutes,
                IsActive = t.IsActive
            }).ToList(),

            AvailableCategories = await _thresholdService.GetAvailableCategoriesAsync(),
            AvailableApps = await _thresholdService.GetAvailableAppsAsync()
        };
    }
}