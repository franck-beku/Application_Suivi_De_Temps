using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Data.Repositories.Implementations;

public class GoalRepository : IGoalRepository
{
    private readonly AppDbContext _context;

    public GoalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Goal> CreateGoalAsync(Goal goal)
    {
        goal.CreatedAt = DateTime.UtcNow;
        _context.Goals.Add(goal);
        await _context.SaveChangesAsync();
        return goal;
    }

    public async Task<Goal?> GetGoalByIdAsync(int id)
    {
        return await _context.Goals.FindAsync(id);
    }

    public async Task<List<Goal>> GetActiveGoalsAsync(int clientId)
    {
        var now = DateTime.UtcNow;
        return await _context.Goals
            .Where(g => g.ClientId == clientId && 
                        g.StartDate <= now && 
                        g.EndDate >= now && 
                        !g.IsCompleted)
            .OrderBy(g => g.EndDate)
            .ToListAsync();
    }

    public async Task<List<Goal>> GetCompletedGoalsAsync(int clientId)
    {
        return await _context.Goals
            .Where(g => g.ClientId == clientId && g.IsCompleted)
            .OrderByDescending(g => g.CompletedAt)
            .ToListAsync();
    }

    public async Task<List<Goal>> GetGoalsByTypeAsync(int clientId, GoalType type)
    {
        return await _context.Goals
            .Where(g => g.ClientId == clientId && g.Type == type)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();
    }

    public async Task<Goal> UpdateGoalAsync(Goal goal)
    {
        _context.Goals.Update(goal);
        await _context.SaveChangesAsync();
        return goal;
    }

    public async Task DeleteGoalAsync(int id)
    {
        var goal = await _context.Goals.FindAsync(id);
        if (goal != null)
        {
            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> GetActiveGoalsCountAsync(int clientId)
    {
        var now = DateTime.UtcNow;
        return await _context.Goals
            .CountAsync(g => g.ClientId == clientId && 
                            g.StartDate <= now && 
                            g.EndDate >= now && 
                            !g.IsCompleted);
    }

    public async Task<int> GetCompletedGoalsCountAsync(int clientId)
    {
        return await _context.Goals
            .CountAsync(g => g.ClientId == clientId && g.IsCompleted);
    }
}