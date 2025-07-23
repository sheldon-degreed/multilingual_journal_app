namespace TranslationService.Models
{
    public class TranslationResponse
    {
        public string OriginalText { get; set; } = string.Empty;
        public string TranslatedText { get; set; } = string.Empty;
        public string SourceLanguage { get; set; } = string.Empty;
        public string TargetLanguage { get; set; } = string.Empty;
        public double Confidence { get; set; } = 1.0;
    }
}