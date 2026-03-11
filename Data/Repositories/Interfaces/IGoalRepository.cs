using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Data.Repositories.Interfaces;

public interface IGoalRepository
{
    Task<Goal> CreateGoalAsync(Goal goal);
    Task<Goal?> GetGoalByIdAsync(int id);
    Task<List<Goal>> GetActiveGoalsAsync(int clientId);
    Task<List<Goal>> GetCompletedGoalsAsync(int clientId);
    Task<List<Goal>> GetGoalsByTypeAsync(int clientId, GoalType type);
    Task<Goal> UpdateGoalAsync(Goal goal);
    Task DeleteGoalAsync(int id);
    Task<int> GetActiveGoalsCountAsync(int clientId);
    Task<int> GetCompletedGoalsCountAsync(int clientId);
}