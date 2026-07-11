using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OptiMail.Models;
using OptiMail.Services;

namespace OptiMail.ViewModels
{
   
    public partial class MainViewModel : ObservableObject
    {
        private readonly IEmailService _emailService;
        private readonly IAiAnalysisService _aiAnalysisService;
        private readonly IProcessedEmailTracker _processedEmailTracker;
        private readonly ICredentialService _credentialService;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string statusMessage = "Hazır";

       
        public ObservableCollection<EmailGroup> GroupedEmails { get; } = new();

        public MainViewModel(
            IEmailService emailService,
            IAiAnalysisService aiAnalysisService,
            IProcessedEmailTracker processedEmailTracker,
            ICredentialService credentialService)
        {
            _emailService = emailService;
            _aiAnalysisService = aiAnalysisService;
            _processedEmailTracker = processedEmailTracker;
            _credentialService = credentialService;
        }

        [RelayCommand]
        private async Task LoadEmailsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var (gmailAddress, gmailAppPassword, geminiApiKey) = await _credentialService.GetCredentialsAsync();

                if (string.IsNullOrWhiteSpace(gmailAddress) ||
                    string.IsNullOrWhiteSpace(gmailAppPassword) ||
                    string.IsNullOrWhiteSpace(geminiApiKey))
                {
                    StatusMessage = "Lütfen önce Ayarlar sayfasından Gmail ve API bilgilerini gir.";
                    return;
                }

                StatusMessage = "E-postalar sunucudan çekiliyor...";

                var processedIds = await _processedEmailTracker.GetProcessedIdsAsync();

                var emails = await _emailService.GetUnreadEmailsAsync(
                    gmailAddress,
                    gmailAppPassword,
                    processedIds,
                    maxCount: 5);

                if (emails.Count == 0)
                {
                    StatusMessage = "Yeni (işlenmemiş) okunmamış mail bulunamadı.";
                    return;
                }

                StatusMessage = "Yapay zeka analizi yapılıyor...";

                foreach (var email in emails)
                {
                    email.Analysis = await _aiAnalysisService.AnalyzeEmailAsync(email.Subject, email.Body, geminiApiKey);

                   
                    await _processedEmailTracker.MarkAsProcessedAsync(email.Id);
                }

                var gruplar = emails
                    .GroupBy(e => e.Analysis?.KategoriBasligi ?? "Diğer")
                    .Select(g => new EmailGroup(g.Key, g.ToList()));

                GroupedEmails.Clear();
                foreach (var grup in gruplar)
                {
                    GroupedEmails.Add(grup);
                }

                StatusMessage = $"{emails.Count} e-posta, {GroupedEmails.Count} kategoriye ayrıldı.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Hata: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
