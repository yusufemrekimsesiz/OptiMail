using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using OptiMail.Models;
using EmailMessage = OptiMail.Models.EmailMessage;

namespace OptiMail.Services
{
    // Gmail (veya standart IMAP) sunucusuna App Password ile bağlanıp
    // okunmamış son e-postaları çeken servis
    public class EmailService : IEmailService
    {
        private const string ImapHost = "imap.gmail.com";
        private const int ImapPort = 993;

        public async Task<List<EmailMessage>> GetUnreadEmailsAsync(string email, string appPassword, ISet<string> excludedIds, int maxCount = 20)
        {
            var result = new List<EmailMessage>();

            using var client = new ImapClient();

            await client.ConnectAsync(ImapHost, ImapPort, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(email, appPassword);

            var inbox = client.Inbox;

            await inbox.OpenAsync(FolderAccess.ReadOnly);

            var uids = await inbox.SearchAsync(SearchQuery.NotSeen);

            
            var candidateUids = uids
                .Where(uid => !excludedIds.Contains(uid.ToString()))
                .ToList();

           
            var targetUids = candidateUids.Count > maxCount
                ? candidateUids.Skip(candidateUids.Count - maxCount).ToList()
                : candidateUids;

            foreach (var uid in targetUids)
            {
                var message = await inbox.GetMessageAsync(uid);

                result.Add(new EmailMessage
                {
                    Id = uid.ToString(),
                    Subject = string.IsNullOrWhiteSpace(message.Subject) ? "(Konu Yok)" : message.Subject,
                    From = message.From.ToString(),
                    Body = !string.IsNullOrWhiteSpace(message.TextBody) ? message.TextBody : (message.HtmlBody ?? string.Empty),
                    ReceivedDate = message.Date.DateTime
                });
            }

            await client.DisconnectAsync(true);

            return result;
        }
    }
}
