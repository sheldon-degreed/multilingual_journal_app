using System.ComponentModel.DataAnnotations;

namespace MultilingualJournalApp.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Language")]
        public string Language { get; set; } = "en";
    }
}