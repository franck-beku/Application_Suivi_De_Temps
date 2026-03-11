namespace Application_Suivi_De_Temps.Models.Enums;

public enum AlertSeverity
{
    Info = 1,       // Information simple
    Warning = 2,    // Avertissement (proche du seuil)
    Critical = 3    // Critique (très au-dessus du seuil)
}