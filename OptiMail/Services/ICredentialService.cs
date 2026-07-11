namespace OptiMail.Services
{
    // Gmail ve Gemini kimlik bilgilerinin SecureStorage üzerinden
    // şifreli okunup yazılmasını yöneten servis
    public interface ICredentialService
    {
        Task SaveCredentialsAsync(string gmailAddress, string gmailAppPassword, string geminiApiKey);
        Task<(string GmailAddress, string GmailAppPassword, string GeminiApiKey)> GetCredentialsAsync();
        Task<bool> HasCredentialsAsync();
    }
}
