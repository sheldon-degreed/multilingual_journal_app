using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MultilingualJournalApp.Data;
using MultilingualJournalApp.Models;

namespace MultilingualJournalApp.Pages.Journal
{
    public class DetailsModel : PageModel
    {
        private readonly JournalContext _context;

        public DetailsModel(JournalContext context)
        {
            _context = context;
        }

        public JournalEntry JournalEntry { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journalentry = await _context.JournalEntries.FirstOrDefaultAsync(m => m.Id == id);
            if (journalentry == null)
            {
                return NotFound();
            }
            else
            {
                JournalEntry = journalentry;
            }
            return Page();
        }
    }
}