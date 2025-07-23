using Microsoft.AspNetCore.Mvc;
using TranslationService.Models;
using TranslationService.Services;

namespace TranslationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranslationController : ControllerBase
    {
        private readonly ITranslationService _translationService;

        public TranslationController(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        [HttpPost("translate")]
        public async Task<ActionResult<TranslationResponse>> Translate([FromBody] TranslationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _translationService.TranslateAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Translation failed", message = ex.Message });
            }
        }

        [HttpGet("languages")]
        public async Task<ActionResult<IEnumerable<string>>> GetSupportedLanguages()
        {
            try
            {
                var languages = await _translationService.GetSupportedLanguagesAsync();
                return Ok(languages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve supported languages", message = ex.Message });
            }
        }

        [HttpPost("batch")]
        public async Task<ActionResult<IEnumerable<TranslationResponse>>> TranslateBatch([FromBody] IEnumerable<TranslationRequest> requests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var tasks = requests.Select(request => _translationService.TranslateAsync(request));
                var results = await Task.WhenAll(tasks);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Batch translation failed", message = ex.Message });
            }
        }
    }
}