namespace OptiMail.Services
{
    // Daha önce yapay zeka ile analiz edilmiş mail ID'lerini yerel olarak takip eder
    // Böylece Gmail'deki gerçek okundu/okunmadı durumu değiştirilmeden
    // aynı mailin tekrar tekrar işlenmesi engellenir
    public interface IProcessedEmailTracker
    {
        Task<HashSet<string>> GetProcessedIdsAsync();
        Task MarkAsProcessedAsync(string emailId);
    }
}
