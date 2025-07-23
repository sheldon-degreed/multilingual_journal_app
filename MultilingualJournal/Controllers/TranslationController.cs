using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultilingualJournal.Data;
using MultilingualJournal.Models;
using System.Text;
using System.Text.Json;

namespace MultilingualJournal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranslationController : ControllerBase
    {
        private readonly JournalContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _translationServiceUrl;

        public TranslationController(JournalContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _translationServiceUrl = configuration.GetValue<string>("TranslationServiceUrl") ?? "http://translation-service:8080";
        }

        [HttpPost("translate-entry/{id}")]
        public async Task<ActionResult<Translation>> TranslateEntry(int id, [FromBody] TranslateEntryRequest request)
        {
            var journalEntry = await _context.JournalEntries.FindAsync(id);
            if (journalEntry == null)
            {
                return NotFound();
            }

            var existingTranslation = await _context.Translations
                .FirstOrDefaultAsync(t => t.JournalEntryId == id && t.TargetLanguage == request.TargetLanguage);

            if (existingTranslation != null)
            {
                return Ok(existingTranslation);
            }

            try
            {
                var translationRequest = new
                {
                    Text = $"{journalEntry.Title}\n\n{journalEntry.Content}",
                    SourceLanguage = journalEntry.Language,
                    TargetLanguage = request.TargetLanguage
                };

                var json = JsonSerializer.Serialize(translationRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_translationServiceUrl}/api/translation/translate", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var translationResponse = JsonSerializer.Deserialize<TranslationResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (translationResponse == null)
                {
                    return StatusCode(500, "Failed to deserialize translation response");
                }

                var parts = translationResponse.TranslatedText.Split("\n\n", 2);
                var translatedTitle = parts[0];
                var translatedContent = parts.Length > 1 ? parts[1] : translationResponse.TranslatedText;

                var translation = new Translation
                {
                    JournalEntryId = id,
                    TargetLanguage = request.TargetLanguage,
                    TranslatedTitle = translatedTitle,
                    TranslatedContent = translatedContent,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Translations.Add(translation);
                await _context.SaveChangesAsync();

                return Ok(translation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Translation failed", message = ex.Message });
            }
        }

        [HttpGet("entry/{id}/translations")]
        public async Task<ActionResult<IEnumerable<Translation>>> GetEntryTranslations(int id)
        {
            var translations = await _context.Translations
                .Where(t => t.JournalEntryId == id)
                .OrderBy(t => t.TargetLanguage)
                .ToListAsync();

            return Ok(translations);
        }

        [HttpDelete("translation/{id}")]
        public async Task<IActionResult> DeleteTranslation(int id)
        {
            var translation = await _context.Translations.FindAsync(id);
            if (translation == null)
            {
                return NotFound();
            }

            _context.Translations.Remove(translation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class TranslateEntryRequest
    {
        public string TargetLanguage { get; set; } = string.Empty;
    }

    public class TranslationResponse
    {
        public string OriginalText { get; set; } = string.Empty;
        public string TranslatedText { get; set; } = string.Empty;
        public string SourceLanguage { get; set; } = string.Empty;
        public string TargetLanguage { get; set; } = string.Empty;
        public double Confidence { get; set; } = 1.0;
    }
}