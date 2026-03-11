using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Services.Profile;
using Application_Suivi_De_Temps.Services.Disconnect;
using Application_Suivi_De_Temps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Suivi_De_Temps.Pages.Profile;

public class IndexModel : PageModel
{
    private readonly IProfileService _profileService;
    private readonly IDisconnectService _disconnectService;
    private const int DefaultClientId = 1;

    public IndexModel(IProfileService profileService, IDisconnectService disconnectService)
    {
        _profileService = profileService;
        _disconnectService = disconnectService;
    }

    [BindProperty]
    public ProfileVm ProfileData { get; set; } = new();

    public DisconnectStatus? DisconnectStatus { get; set; }
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var client = await _profileService.GetClientAsync(DefaultClientId);
        if (client == null)
        {
            ErrorMessage = "Client introuvable.";
            return Page();
        }

        var preferences = await _profileService.GetPreferencesAsync(DefaultClientId);

        // Mapper les données vers le ViewModel
        ProfileData = new ProfileVm
        {
            DisplayName = client.DisplayName,
            DefaultDailyGoalMinutes = preferences?.DefaultDailyGoalMinutes ?? 180,
            EnableNotifications = preferences?.EnableNotifications ?? true,
            EnableBreakReminders = preferences?.EnableBreakReminders ?? true,
            EnableWeeklyReport = preferences?.EnableWeeklyReport ?? true,
            DisconnectStartTime = preferences?.DisconnectStartTime,
            DisconnectEndTime = preferences?.DisconnectEndTime,
            EnableDisconnectMode = preferences?.EnableDisconnectMode ?? false
        };

        // Obtenir le statut de déconnexion
        DisconnectStatus = await _disconnectService.GetDisconnectStatusAsync(DefaultClientId);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = "Veuillez corriger les erreurs dans le formulaire.";
            DisconnectStatus = await _disconnectService.GetDisconnectStatusAsync(DefaultClientId);
            return Page();
        }

        try
        {
            // Mettre à jour le nom d'affichage
            await _profileService.UpdateClientNameAsync(DefaultClientId, ProfileData.DisplayName);

            // Mettre à jour les préférences
            var preferences = new UserPreferences
            {
                ClientId = DefaultClientId,
                DefaultDailyGoalMinutes = ProfileData.DefaultDailyGoalMinutes,
                EnableNotifications = ProfileData.EnableNotifications,
                EnableBreakReminders = ProfileData.EnableBreakReminders,
                EnableWeeklyReport = ProfileData.EnableWeeklyReport,
                DisconnectStartTime = ProfileData.DisconnectStartTime,
                DisconnectEndTime = ProfileData.DisconnectEndTime,
                EnableDisconnectMode = ProfileData.EnableDisconnectMode
            };

            await _profileService.UpdatePreferencesAsync(DefaultClientId, preferences);

            SuccessMessage = "Profil mis à jour avec succès !";
            TempData["SuccessMessage"] = SuccessMessage;
            
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur lors de la mise à jour : {ex.Message}";
            DisconnectStatus = await _disconnectService.GetDisconnectStatusAsync(DefaultClientId);
            return Page();
        }
    }
}