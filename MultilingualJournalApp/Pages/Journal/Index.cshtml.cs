using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MultilingualJournalApp.Data;
using MultilingualJournalApp.Models;

namespace MultilingualJournalApp.Pages.Journal
{
    public class IndexModel : PageModel
    {
        private readonly JournalContext _context;

        public IndexModel(JournalContext context)
        {
            _context = context;
        }

        public IList<JournalEntry> JournalEntries { get; set; } = default!;

        public async Task OnGetAsync()
        {
            JournalEntries = await _context.JournalEntries
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }
    }
}