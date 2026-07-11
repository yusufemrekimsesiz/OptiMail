namespace OptiMail.Services
{
    // .NET MAUI'nin SecureStorage API'si, verileri Android Keystore (donanım destekli
    // şifreleme) kullanarak saklar. Kod içine düz metin şifre/anahtar yazmak yerine
    // kullanıcı bu bilgileri bir kere girer, uygulama şifreli şekilde cihazda tutar
    public class CredentialService : ICredentialService
    {
        private const string GmailAddressKey = "OptiMail_GmailAddress";
        private const string GmailAppPasswordKey = "OptiMail_GmailAppPassword";
        private const string GeminiApiKeyKey = "OptiMail_GeminiApiKey";

        public async Task SaveCredentialsAsync(string gmailAddress, string gmailAppPassword, string geminiApiKey)
        {
            await SecureStorage.Default.SetAsync(GmailAddressKey, gmailAddress);
            await SecureStorage.Default.SetAsync(GmailAppPasswordKey, gmailAppPassword);
            await SecureStorage.Default.SetAsync(GeminiApiKeyKey, geminiApiKey);
        }

        public async Task<(string GmailAddress, string GmailAppPassword, string GeminiApiKey)> GetCredentialsAsync()
        {
            var gmailAddress = await SecureStorage.Default.GetAsync(GmailAddressKey) ?? string.Empty;
            var gmailAppPassword = await SecureStorage.Default.GetAsync(GmailAppPasswordKey) ?? string.Empty;
            var geminiApiKey = await SecureStorage.Default.GetAsync(GeminiApiKeyKey) ?? string.Empty;

            return (gmailAddress, gmailAppPassword, geminiApiKey);
        }

        public async Task<bool> HasCredentialsAsync()
        {
            var (gmailAddress, gmailAppPassword, geminiApiKey) = await GetCredentialsAsync();

            return !string.IsNullOrWhiteSpace(gmailAddress)
                && !string.IsNullOrWhiteSpace(gmailAppPassword)
                && !string.IsNullOrWhiteSpace(geminiApiKey);
        }
    }
}
