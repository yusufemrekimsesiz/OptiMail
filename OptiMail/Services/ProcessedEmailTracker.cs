namespace OptiMail.Services
{
    // .NET MAUI'nin Preferences API'si ile cihazda kalıcı, basit key-value depolama kullanır
    // ID'ler virgülle ayrılmış tek bir metin olarak saklanır
    public class ProcessedEmailTracker : IProcessedEmailTracker
    {
        private const string StorageKey = "OptiMail_ProcessedEmailIds";

        public Task<HashSet<string>> GetProcessedIdsAsync()
        {
            var raw = Preferences.Default.Get(StorageKey, string.Empty);

            var ids = string.IsNullOrWhiteSpace(raw)
                ? new HashSet<string>()
                : raw.Split(',', StringSplitOptions.RemoveEmptyEntries).ToHashSet();

            return Task.FromResult(ids);
        }

        public async Task MarkAsProcessedAsync(string emailId)
        {
            var current = await GetProcessedIdsAsync();
            current.Add(emailId);

            // Liste sınırsız büyümesin diye en fazla son 500 ID'yi tut
            var trimmed = current.Count > 500
                ? current.Skip(current.Count - 500)
                : current;

            Preferences.Default.Set(StorageKey, string.Join(',', trimmed));
        }
    }
}
