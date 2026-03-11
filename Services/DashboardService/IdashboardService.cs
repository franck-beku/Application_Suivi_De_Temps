using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Services.Dashboard;

public interface IDashboardService
{
    /// <summary>
    /// Obtenir toutes les données du dashboard pour un client
    /// </summary>
    Task<DashboardData> GetDashboardDataAsync(int clientId);
}