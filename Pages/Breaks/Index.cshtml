using Application_Suivi_De_Temps.Services.Breaks;
using Application_Suivi_De_Temps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Suivi_De_Temps.Pages.Breaks;

public class IndexModel : PageModel
{
    private readonly IBreakService _breakService;
    private const int DefaultClientId = 1;

    public IndexModel(IBreakService breakService)
    {
        _breakService = breakService;
    }

    public BreaksVm ViewModel { get; set; } = new();
    
    [BindProperty]
    public RecordBreakInputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostRecordBreakAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDataAsync();
            return Page();
        }

        try
        {
            await _breakService.RecordBreakSessionAsync(
                DefaultClientId, 
                Input.ActivityId, 
                Input.DurationMinutes, 
                Input.Notes);
            
            TempData["SuccessMessage"] = $"✅ Pause enregistrée : {Input.DurationMinutes} min";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Erreur : {ex.Message}";
            await LoadDataAsync();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostQuickBreakAsync(int activityId, int duration)
    {
        try
        {
            await _breakService.RecordBreakSessionAsync(DefaultClientId, activityId, duration);
            TempData["SuccessMessage"] = $"✅ Pause enregistrée : {duration} min";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Erreur : {ex.Message}";
            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostClearSessionsAsync()
    {
        await _breakService.DeleteAllSessionsAsync(DefaultClientId);
        TempData["SuccessMessage"] = "Toutes les sessions ont été supprimées";
        return RedirectToPage();
    }

    private async Task LoadDataAsync()
    {
        ViewModel = new BreaksVm
        {
            AllActivities = await _breakService.GetAllActivitiesAsync(),
            TodaySessions = await _breakService.GetTodaySessionsAsync(DefaultClientId),
            Stats = await _breakService.GetBreakStatsAsync(DefaultClientId)
        };
        
        if (ViewModel.AllActivities.Any())
        {
            Input.ActivityId = ViewModel.AllActivities.First().Id;
        }
    }
}