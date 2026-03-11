using Application_Suivi_De_Temps.Services.Tips;
using Application_Suivi_De_Temps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Suivi_De_Temps.Pages.Tips;

public class IndexModel : PageModel
{
    private readonly ITipService _tipService;

    public IndexModel(ITipService tipService)
    {
        _tipService = tipService;
    }

    public TipsVm ViewModel { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDataAsync();
        return Page();
    }

    private async Task LoadDataAsync()
    {
        ViewModel = new TipsVm
        {
            AllTips = await _tipService.GetAllTipsAsync(),
            TipOfTheDay = await _tipService.GetTipOfTheDayAsync()
        };
    }
}