using Application_Suivi_De_Temps.Services.Goals;
using Application_Suivi_De_Temps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Suivi_De_Temps.Pages.Goals;

public class IndexModel : PageModel
{
    private readonly IGoalService _goalService;
    private const int DefaultClientId = 1;

    public IndexModel(IGoalService goalService)
    {
        _goalService = goalService;
    }

    public GoalsVm ViewModel { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDataAsync();
        return Page();
    }

    private async Task LoadDataAsync()
    {
        var activeGoals = await _goalService.GetActiveGoalsAsync(DefaultClientId);
        var completedGoals = await _goalService.GetCompletedGoalsAsync(DefaultClientId);
        var stats = await _goalService.GetGoalStatsAsync(DefaultClientId);

        ViewModel = new GoalsVm
        {
            ActiveGoals = activeGoals,
            CompletedGoals = completedGoals,
            ActiveCount = stats.active,
            CompletedCount = stats.completed,
            SuccessRate = stats.successRate
        };
    }
}