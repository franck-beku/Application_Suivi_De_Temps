using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Data.Repositories.Interfaces;

public interface IAppBlockRepository
{
    Task<AppBlock> CreateBlockAsync(AppBlock block);
    Task<AppBlock?> GetActiveBlockAsync(int clientId, int appId);
    Task<List<AppBlock>> GetActiveBlocksAsync(int clientId);
    Task<List<AppBlock>> GetAllBlocksAsync(int clientId);
    Task DeleteBlockAsync(int blockId);
    Task DeleteExpiredBlocksAsync(int clientId);
    Task DeleteAllBlocksAsync(int clientId);
}
