using TranslationService.Models;

namespace TranslationService.Services
{
    public class MockTranslationService : ITranslationService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _translations = new()
        {
            ["en"] = new Dictionary<string, string>
            {
                ["es"] = "Spanish translation of: ",
                ["fr"] = "French translation of: ",
                ["de"] = "German translation of: ",
                ["it"] = "Italian translation of: ",
                ["pt"] = "Portuguese translation of: "
            },
            ["es"] = new Dictionary<string, string>
            {
                ["en"] = "English translation of: ",
                ["fr"] = "French translation of: ",
                ["de"] = "German translation of: ",
                ["it"] = "Italian translation of: ",
                ["pt"] = "Portuguese translation of: "
            }
        };

        private readonly string[] _supportedLanguages = { "en", "es", "fr", "de", "it", "pt" };

        public async Task<TranslationResponse> TranslateAsync(TranslationRequest request)
        {
            await Task.Delay(100);

            if (request.SourceLanguage == request.TargetLanguage)
            {
                return new TranslationResponse
                {
                    OriginalText = request.Text,
                    TranslatedText = request.Text,
                    SourceLanguage = request.SourceLanguage,
                    TargetLanguage = request.TargetLanguage,
                    Confidence = 1.0
                };
            }

            var prefix = _translations.ContainsKey(request.SourceLanguage) && 
                        _translations[request.SourceLanguage].ContainsKey(request.TargetLanguage)
                        ? _translations[request.SourceLanguage][request.TargetLanguage]
                        : $"Translation from {request.SourceLanguage} to {request.TargetLanguage}: ";

            return new TranslationResponse
            {
                OriginalText = request.Text,
                TranslatedText = prefix + request.Text,
                SourceLanguage = request.SourceLanguage,
                TargetLanguage = request.TargetLanguage,
                Confidence = 0.85
            };
        }

        public async Task<IEnumerable<string>> GetSupportedLanguagesAsync()
        {
            await Task.Delay(10);
            return _supportedLanguages;
        }
    }
}