namespace OptiMail.Models
{
    // IMAP'ten çekilen ham e-posta verisini temsil eder
    public class EmailMessage
    {
        public string Id { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime ReceivedDate { get; set; }

        // Yapay zeka analizi tamamlanana kadar null kalır
        public EmailAnalysisResult? Analysis { get; set; }
    }
}
