using System.Text.Json.Serialization;

namespace OptiMail.Models
{
    // LLM API'sinden dönen JSON'ın eşleştiği veri sözleşmesi (data contract)
    public class EmailAnalysisResult
    {
        [JsonPropertyName("kategori_basligi")]
        public string KategoriBasligi { get; set; } = "Genel";

        [JsonPropertyName("ozet")]
        public string Ozet { get; set; } = string.Empty;

        [JsonPropertyName("aciliyet")]
        public string Aciliyet { get; set; } = "Orta"; // Düşük, Orta, Yüksek

        [JsonPropertyName("aksiyon_maddeleri")]
        public List<string> AksiyonMaddeleri { get; set; } = new();
    }
}
