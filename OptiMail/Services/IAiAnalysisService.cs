using OptiMail.Models;

namespace OptiMail.Services
{
    public interface IAiAnalysisService
    {
        Task<EmailAnalysisResult> AnalyzeEmailAsync(string subject, string body, string apiKey);
    }
}
