using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Services.AppBlocks;

public class AppBlockService : IAppBlockService
{
    private readonly IAppBlockRepository _blockRepo;

    public AppBlockService(IAppBlockRepository blockRepo)
    {
        _blockRepo = blockRepo;
    }

    public async Task<AppBlock> BlockAppAsync(int clientId, int appId, int durationMinutes, string? reason = null)
    {
        // Vérifier s'il existe déjà un blocage actif
        var existingBlock = await _blockRepo.GetActiveBlockAsync(clientId, appId);
        
        if (existingBlock != null)
        {
            // Supprimer l'ancien et créer un nouveau
            await _blockRepo.DeleteBlockAsync(existingBlock.Id);
        }

        var block = new AppBlock
        {
            ClientId = clientId,
            AppId = appId,
            BlockedUntil = DateTime.UtcNow.AddMinutes(durationMinutes),
            Reason = reason
        };

        return await _blockRepo.CreateBlockAsync(block);
    }

    public async Task UnblockAppAsync(int clientId, int appId)
    {
        var block = await _blockRepo.GetActiveBlockAsync(clientId, appId);
        
        if (block != null)
        {
            await _blockRepo.DeleteBlockAsync(block.Id);
        }
    }

    public async Task<bool> IsAppBlockedAsync(int clientId, int appId)
    {
        var block = await _blockRepo.GetActiveBlockAsync(clientId, appId);
        return block != null && !block.IsExpired;
    }

    public async Task<List<AppBlock>> GetActiveBlocksAsync(int clientId)
    {
        // Nettoyer les blocages expirés d'abord
        await _blockRepo.DeleteExpiredBlocksAsync(clientId);
        
        return await _blockRepo.GetActiveBlocksAsync(clientId);
    }

    public async Task CleanExpiredBlocksAsync(int clientId)
    {
        await _blockRepo.DeleteExpiredBlocksAsync(clientId);
    }

    public async Task ClearAllBlocksAsync(int clientId)
    {
        await _blockRepo.DeleteAllBlocksAsync(clientId);
    }
}