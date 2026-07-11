using OptiMail.Models;
using EmailMessage = OptiMail.Models.EmailMessage;

namespace OptiMail.Services
{
    
    public interface IEmailService
    {
       
        Task<List<EmailMessage>> GetUnreadEmailsAsync(string email, string appPassword, ISet<string> excludedIds, int maxCount = 20);
    }
}
