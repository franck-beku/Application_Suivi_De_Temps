using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Data.Repositories.Interfaces;

public interface IUsageRepository
{
    /// <summary>
    /// Obtenir une entrée d'usage pour un client, une app et une date spécifiques
    /// </summary>
    Task<UsageEntry?> GetUsageEntryAsync(int clientId, int appId, DateOnly date);
    
    /// <summary>
    /// Obtenir toutes les entrées d'usage pour un client à une date donnée
    /// </summary>
    Task<List<UsageEntry>> GetDailyUsageAsync(int clientId, DateOnly date);
    
    /// <summary>
    /// Obtenir toutes les entrées d'usage pour un client sur une période
    /// </summary>
    Task<List<UsageEntry>> GetUsageRangeAsync(int clientId, DateOnly startDate, DateOnly endDate);
    
    /// <summary>
    /// Ajouter ou mettre à jour une entrée d'usage
    /// </summary>
    Task<UsageEntry> AddOrUpdateUsageAsync(int clientId, int appId, DateOnly date, int minutesToAdd);
    
    /// <summary>
    /// Créer une nouvelle entrée d'usage
    /// </summary>
    Task<UsageEntry> CreateUsageEntryAsync(UsageEntry entry);
    
    /// <summary>
    /// Mettre à jour une entrée d'usage existante
    /// </summary>
    Task<UsageEntry> UpdateUsageEntryAsync(UsageEntry entry);
    
    /// <summary>
    /// Supprimer toutes les entrées d'usage d'un client
    /// </summary>
    Task DeleteAllUsageAsync(int clientId);
}