using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Models;

public class Category
{
    public int Id { get; set; }

    public AppCategory Type { get; set; }

    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<App> Apps { get; set; } = new List<App>();
}
