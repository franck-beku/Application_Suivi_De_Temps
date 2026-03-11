using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Data.Repositories.Implementations;

public class AppBlockRepository : IAppBlockRepository
{
    private readonly AppDbContext _context;

    public AppBlockRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AppBlock> CreateBlockAsync(AppBlock block)
    {
        block.CreatedAt = DateTime.UtcNow;
        _context.AppBlocks.Add(block);
        await _context.SaveChangesAsync();
        
        // Recharger avec les relations
        return await _context.AppBlocks
            .Include(b => b.App)
                .ThenInclude(a => a!.Category)
            .FirstOrDefaultAsync(b => b.Id == block.Id) ?? block;
    }

    public async Task<AppBlock?> GetActiveBlockAsync(int clientId, int appId)
    {
        var now = DateTime.UtcNow;
        
        return await _context.AppBlocks
            .Include(b => b.App)
                .ThenInclude(a => a!.Category)
            .FirstOrDefaultAsync(b => 
                b.ClientId == clientId && 
                b.AppId == appId && 
                b.BlockedUntil > now);
    }

    public async Task<List<AppBlock>> GetActiveBlocksAsync(int clientId)
    {
        var now = DateTime.UtcNow;
        
        return await _context.AppBlocks
            .Include(b => b.App)
                .ThenInclude(a => a!.Category)
            .Where(b => b.ClientId == clientId && b.BlockedUntil > now)
            .OrderBy(b => b.BlockedUntil)
            .ToListAsync();
    }

    public async Task<List<AppBlock>> GetAllBlocksAsync(int clientId)
    {
        return await _context.AppBlocks
            .Include(b => b.App)
                .ThenInclude(a => a!.Category)
            .Where(b => b.ClientId == clientId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task DeleteBlockAsync(int blockId)
    {
        var block = await _context.AppBlocks.FindAsync(blockId);
        if (block != null)
        {
            _context.AppBlocks.Remove(block);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteExpiredBlocksAsync(int clientId)
    {
        var now = DateTime.UtcNow;
        var expiredBlocks = await _context.AppBlocks
            .Where(b => b.ClientId == clientId && b.BlockedUntil <= now)
            .ToListAsync();

        _context.AppBlocks.RemoveRange(expiredBlocks);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllBlocksAsync(int clientId)
    {
        var blocks = await _context.AppBlocks
            .Where(b => b.ClientId == clientId)
            .ToListAsync();

        _context.AppBlocks.RemoveRange(blocks);
        await _context.SaveChangesAsync();
    }
}