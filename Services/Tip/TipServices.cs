using Application_Suivi_De_Temps.Data;
using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Services.Tips;

public class TipService : ITipService
{
    private readonly AppDbContext _context;

    public TipService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tip>> GetAllTipsAsync()
    {
        return await _context.Tips
            .Where(t => t.IsActive)
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Title)
            .ToListAsync();
    }

    public async Task<List<Tip>> GetTipsByCategoryAsync(TipCategory category)
    {
        return await _context.Tips
            .Where(t => t.IsActive && t.Category == category)
            .OrderBy(t => t.Title)
            .ToListAsync();
    }

    public async Task<Tip?> GetRandomTipAsync()
    {
        var tips = await _context.Tips
            .Where(t => t.IsActive)
            .ToListAsync();

        if (!tips.Any())
            return null;

        var random = new Random();
        var index = random.Next(tips.Count);
        return tips[index];
    }

    public async Task<Tip?> GetTipOfTheDayAsync()
    {
        // Utilise la date du jour pour sélectionner un conseil cohérent toute la journée
        var tips = await _context.Tips
            .Where(t => t.IsActive)
            .OrderBy(t => t.Id)
            .ToListAsync();

        if (!tips.Any())
            return null;

        // Utilise le jour de l'année pour avoir un conseil différent chaque jour
        var dayOfYear = DateTime.Today.DayOfYear;
        var index = dayOfYear % tips.Count;
        return tips[index];
    }
}