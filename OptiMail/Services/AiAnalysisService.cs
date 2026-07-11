using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using OptiMail.Models;

namespace OptiMail.Services
{

    public class AiAnalysisService : IAiAnalysisService
    {
        private readonly HttpClient _httpClient;

        private const string Model = "gemini-2.5-flash";
        private const string ApiUrlTemplate =
            "https://generativelanguage.googleapis.com/v1beta/models/{0}:generateContent?key={1}";

        private const int MaxRetries = 3;


        private const string SystemPrompt =
            "Sen bir e-posta analiz asistanısın. Sana verilen e-postanın başlığını ve içeriğini analiz et. " +
            "SADECE aşağıdaki alanlara sahip geçerli bir JSON nesnesi döndür, başka hiçbir açıklama, " +
            "markdown işareti veya ek metin ekleme:\n" +
            "{\n" +
            "  \"kategori_basligi\": \"(2-3 kelimelik kategori, örn: 'Fatura Bildirimi')\",\n" +
            "  \"ozet\": \"(2-3 cümlelik özet)\",\n" +
            "  \"aciliyet\": \"(Düşük, Orta veya Yüksek)\",\n" +
            "  \"aksiyon_maddeleri\": [\"(varsa yapılacaklar, yoksa boş dizi)\"]\n" +
            "}";

        public AiAnalysisService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmailAnalysisResult> AnalyzeEmailAsync(string subject, string body, string apiKey)
        {
            var userContent = $"Başlık: {subject}\nİçerik: {body}";
            var apiUrl = string.Format(ApiUrlTemplate, Model, apiKey);

            var requestBody = new
            {
                system_instruction = new
                {
                    parts = new[] { new { text = SystemPrompt } }
                },
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[] { new { text = userContent } }
                    }
                },
                generationConfig = new
                {
                    response_mime_type = "application/json"
                }
            };

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = JsonContent.Create(requestBody)
                };

                var response = await _httpClient.SendAsync(request);


                if (response.StatusCode == (HttpStatusCode)429)
                {
                    if (attempt == MaxRetries)
                    {
                        var errorBody = await response.Content.ReadAsStringAsync();
                        throw new Exception($"API Hatası ({response.StatusCode}): {errorBody}");
                    }

                    await Task.Delay(TimeSpan.FromSeconds(20));
                    continue;
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API Hatası ({response.StatusCode}): {errorBody}");
                }

                var responseJson = await response.Content.ReadFromJsonAsync<JsonElement>();

                var rawText = responseJson
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString() ?? "{}";


                var cleanJson = rawText.Replace("```json", "").Replace("```", "").Trim();

                var analysis = JsonSerializer.Deserialize<EmailAnalysisResult>(
                    cleanJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return analysis ?? new EmailAnalysisResult();
            }

            return new EmailAnalysisResult();
        }
    }
}

