using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Services.Goals;

public interface IGoalService
{
    /// <summary>
    /// Créer un nouvel objectif
    /// </summary>
    Task<Goal> CreateGoalAsync(int clientId, GoalType type, string title, string description, int targetValue, DateTime startDate, DateTime endDate);
    
    /// <summary>
    /// Obtenir tous les objectifs actifs
    /// </summary>
    Task<List<Goal>> GetActiveGoalsAsync(int clientId);
    
    /// <summary>
    /// Obtenir tous les objectifs complétés
    /// </summary>
    Task<List<Goal>> GetCompletedGoalsAsync(int clientId);
    
    /// <summary>
    /// Incrémenter la progression d'un objectif
    /// </summary>
    Task<Goal> IncrementProgressAsync(int goalId, int incrementBy = 1);
    
    /// <summary>
    /// Marquer un objectif comme complété
    /// </summary>
    Task<Goal> CompleteGoalAsync(int goalId);
    
    /// <summary>
    /// Obtenir les statistiques des objectifs
    /// </summary>
    Task<(int active, int completed, int successRate)> GetGoalStatsAsync(int clientId);
}