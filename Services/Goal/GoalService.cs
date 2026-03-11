using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Services.Goals;

public class GoalService : IGoalService
{
    private readonly IGoalRepository _goalRepo;

    public GoalService(IGoalRepository goalRepo)
    {
        _goalRepo = goalRepo;
    }

    public async Task<Goal> CreateGoalAsync(int clientId, GoalType type, string title, string description, int targetValue, DateTime startDate, DateTime endDate)
    {
        var goal = new Goal
        {
            ClientId = clientId,
            Type = type,
            Title = title,
            Description = description,
            TargetValue = targetValue,
            CurrentValue = 0,
            StartDate = startDate,
            EndDate = endDate,
            IsCompleted = false
        };

        return await _goalRepo.CreateGoalAsync(goal);
    }

    public async Task<List<Goal>> GetActiveGoalsAsync(int clientId)
    {
        return await _goalRepo.GetActiveGoalsAsync(clientId);
    }

    public async Task<List<Goal>> GetCompletedGoalsAsync(int clientId)
    {
        return await _goalRepo.GetCompletedGoalsAsync(clientId);
    }

    public async Task<Goal> IncrementProgressAsync(int goalId, int incrementBy = 1)
    {
        var goal = await _goalRepo.GetGoalByIdAsync(goalId);
        
        if (goal == null)
            throw new Exception($"Objectif {goalId} introuvable");

        goal.CurrentValue += incrementBy;

        // Si objectif atteint, le marquer comme complété
        if (goal.CurrentValue >= goal.TargetValue && !goal.IsCompleted)
        {
            goal.IsCompleted = true;
            goal.CompletedAt = DateTime.UtcNow;
        }

        return await _goalRepo.UpdateGoalAsync(goal);
    }

    public async Task<Goal> CompleteGoalAsync(int goalId)
    {
        var goal = await _goalRepo.GetGoalByIdAsync(goalId);
        
        if (goal == null)
            throw new Exception($"Objectif {goalId} introuvable");

        if (!goal.IsCompleted)
        {
            goal.IsCompleted = true;
            goal.CompletedAt = DateTime.UtcNow;
            goal.CurrentValue = goal.TargetValue;
        }

        return await _goalRepo.UpdateGoalAsync(goal);
    }

    public async Task<(int active, int completed, int successRate)> GetGoalStatsAsync(int clientId)
    {
        var activeCount = await _goalRepo.GetActiveGoalsCountAsync(clientId);
        var completedCount = await _goalRepo.GetCompletedGoalsCountAsync(clientId);
        
        var totalGoals = activeCount + completedCount;
        var successRate = totalGoals > 0 ? (int)((completedCount / (double)totalGoals) * 100) : 0;

        return (activeCount, completedCount, successRate);
    }
}