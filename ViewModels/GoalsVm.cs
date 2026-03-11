using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.ViewModels;

public class GoalsVm
{
    public List<Goal> ActiveGoals { get; set; } = new();
    public List<Goal> CompletedGoals { get; set; } = new();
    public int ActiveCount { get; set; }
    public int CompletedCount { get; set; }
    public int SuccessRate { get; set; }
}