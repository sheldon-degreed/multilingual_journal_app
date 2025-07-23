using System.ComponentModel.DataAnnotations;

namespace TranslationService.Models
{
    public class TranslationRequest
    {
        [Required]
        public string Text { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(10)]
        public string SourceLanguage { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(10)]
        public string TargetLanguage { get; set; } = string.Empty;
    }
}