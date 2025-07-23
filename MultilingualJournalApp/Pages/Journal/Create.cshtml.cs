using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MultilingualJournalApp.Data;
using MultilingualJournalApp.Models;

namespace MultilingualJournalApp.Pages.Journal
{
    public class CreateModel : PageModel
    {
        private readonly JournalContext _context;

        public CreateModel(JournalContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public JournalEntry JournalEntry { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            JournalEntry.CreatedDate = DateTime.Now;
            _context.JournalEntries.Add(JournalEntry);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}