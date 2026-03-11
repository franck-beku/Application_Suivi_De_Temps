using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.ViewModels;

public class TipsVm
{
    public List<Tip> AllTips { get; set; } = new();
    public Tip? TipOfTheDay { get; set; }
    
    public Dictionary<TipCategory, List<Tip>> TipsByCategory
    {
        get
        {
            return AllTips
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}