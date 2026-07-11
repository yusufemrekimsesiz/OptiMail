using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OptiMail.Services;

namespace OptiMail.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ICredentialService _credentialService;

        [ObservableProperty]
        private string gmailAddress = string.Empty;

        [ObservableProperty]
        private string gmailAppPassword = string.Empty;

        [ObservableProperty]
        private string geminiApiKey = string.Empty;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        public SettingsViewModel(ICredentialService credentialService)
        {
            _credentialService = credentialService;
        }

        public async Task LoadAsync()
        {
            var (address, password, apiKey) = await _credentialService.GetCredentialsAsync();
            GmailAddress = address;
            GmailAppPassword = password;
            GeminiApiKey = apiKey;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(GmailAddress) ||
                string.IsNullOrWhiteSpace(GmailAppPassword) ||
                string.IsNullOrWhiteSpace(GeminiApiKey))
            {
                StatusMessage = "Lütfen tüm alanları doldur.";
                return;
            }

            await _credentialService.SaveCredentialsAsync(GmailAddress, GmailAppPassword, GeminiApiKey);
            StatusMessage = "Kaydedildi. Ana sayfaya geri dönebilirsin.";
        }
    }
}
