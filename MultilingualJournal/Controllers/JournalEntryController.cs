using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultilingualJournal.Data;
using MultilingualJournal.Models;

namespace MultilingualJournal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JournalEntryController : ControllerBase
    {
        private readonly JournalContext _context;

        public JournalEntryController(JournalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JournalEntry>>> GetJournalEntries()
        {
            return await _context.JournalEntries
                .Include(j => j.Tags)
                .Include(j => j.Translations)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JournalEntry>> GetJournalEntry(int id)
        {
            var journalEntry = await _context.JournalEntries
                .Include(j => j.Tags)
                .Include(j => j.Translations)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (journalEntry == null)
            {
                return NotFound();
            }

            return journalEntry;
        }

        [HttpPost]
        public async Task<ActionResult<JournalEntry>> PostJournalEntry(JournalEntry journalEntry)
        {
            journalEntry.CreatedAt = DateTime.UtcNow;
            journalEntry.UpdatedAt = DateTime.UtcNow;

            _context.JournalEntries.Add(journalEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJournalEntry", new { id = journalEntry.Id }, journalEntry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutJournalEntry(int id, JournalEntry journalEntry)
        {
            if (id != journalEntry.Id)
            {
                return BadRequest();
            }

            journalEntry.UpdatedAt = DateTime.UtcNow;
            _context.Entry(journalEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JournalEntryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJournalEntry(int id)
        {
            var journalEntry = await _context.JournalEntries.FindAsync(id);
            if (journalEntry == null)
            {
                return NotFound();
            }

            _context.JournalEntries.Remove(journalEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<JournalEntry>>> SearchJournalEntries(string? query, string? language)
        {
            var entries = _context.JournalEntries
                .Include(j => j.Tags)
                .Include(j => j.Translations)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                entries = entries.Where(j => j.Title.Contains(query) || j.Content.Contains(query));
            }

            if (!string.IsNullOrEmpty(language))
            {
                entries = entries.Where(j => j.Language == language);
            }

            return await entries.OrderByDescending(j => j.CreatedAt).ToListAsync();
        }

        private bool JournalEntryExists(int id)
        {
            return _context.JournalEntries.Any(e => e.Id == id);
        }
    }
}