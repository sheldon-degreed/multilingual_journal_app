using System.ComponentModel.DataAnnotations;

namespace MultilingualJournal.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(10)]
        public string Language { get; set; } = "en";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public List<Tag> Tags { get; set; } = new List<Tag>();
        
        public List<Translation> Translations { get; set; } = new List<Translation>();
    }
}