using TranslationService.Models;

namespace TranslationService.Services
{
    public interface ITranslationService
    {
        Task<TranslationResponse> TranslateAsync(TranslationRequest request);
        Task<IEnumerable<string>> GetSupportedLanguagesAsync();
    }
}