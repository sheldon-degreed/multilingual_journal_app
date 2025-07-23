using System.ComponentModel.DataAnnotations;

namespace MultilingualJournal.Models
{
    public class Translation
    {
        public int Id { get; set; }
        
        public int JournalEntryId { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string TargetLanguage { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string TranslatedTitle { get; set; } = string.Empty;
        
        [Required]
        public string TranslatedContent { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public JournalEntry JournalEntry { get; set; } = null!;
    }
}