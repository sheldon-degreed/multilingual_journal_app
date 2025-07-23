using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MultilingualJournalApp.Data;
using MultilingualJournalApp.Models;

namespace MultilingualJournalApp.Pages.Journal
{
    public class DeleteModel : PageModel
    {
        private readonly JournalContext _context;

        public DeleteModel(JournalContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journalentry = await _context.JournalEntries.FindAsync(id);
            if (journalentry != null)
            {
                JournalEntry = journalentry;
                _context.JournalEntries.Remove(JournalEntry);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}