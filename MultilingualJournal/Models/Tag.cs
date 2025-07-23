using System.ComponentModel.DataAnnotations;

namespace MultilingualJournal.Models
{
    public class Tag
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(7)]
        public string Color { get; set; } = "#007bff";
        
        public bool IsMilestone { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public List<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
    }
}