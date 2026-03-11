using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.ViewModels;

public class DashboardVm
{
    public DashboardData Data { get; set; } = new();
    
    // Helpers pour l'affichage
    public bool ShowOverGoalAlert => Data.IsOverGoal;
    public bool ShowDisconnectAlert => Data.DisconnectStatus?.IsInDisconnectPeriod ?? false;
    
    public string GetAppIconClass(string appName)
    {
        return appName.ToLower() switch
        {
            "instagram" => "bi-instagram",
            "tiktok" => "bi-tiktok",
            "youtube" => "bi-youtube",
            "snapchat" => "bi-snapchat",
            "whatsapp" => "bi-whatsapp",
            "facebook" => "bi-facebook",
            "twitter" => "bi-twitter",
            "messenger" => "bi-messenger",
            _ => "bi-phone"
        };
    }
    
    public string GetAppColor(string appName)
    {
        return appName.ToLower() switch
        {
            "instagram" => "#E4405F",
            "tiktok" => "#000000",
            "youtube" => "#FF0000",
            "snapchat" => "#FFFC00",
            "whatsapp" => "#25D366",
            "facebook" => "#1877F2",
            "twitter" => "#1DA1F2",
            _ => "#6c757d"
        };
    }
}