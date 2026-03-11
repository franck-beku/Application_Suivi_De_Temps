using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Services.Tips;

public interface ITipService
{
    /// <summary>
    /// Obtenir tous les conseils actifs
    /// </summary>
    Task<List<Tip>> GetAllTipsAsync();
    
    /// <summary>
    /// Obtenir les conseils par catégorie
    /// </summary>
    Task<List<Tip>> GetTipsByCategoryAsync(TipCategory category);
    
    /// <summary>
    /// Obtenir un conseil aléatoire
    /// </summary>
    Task<Tip?> GetRandomTipAsync();
    
    /// <summary>
    /// Obtenir le conseil du jour (basé sur la date)
    /// </summary>
    Task<Tip?> GetTipOfTheDayAsync();
}