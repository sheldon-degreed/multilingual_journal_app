using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultilingualJournal.Data;
using MultilingualJournal.Models;

namespace MultilingualJournal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly JournalContext _context;

        public TagController(JournalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            return await _context.Tags
                .Include(t => t.JournalEntries)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        [HttpGet("milestones")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetMilestoneTags()
        {
            return await _context.Tags
                .Where(t => t.IsMilestone)
                .Include(t => t.JournalEntries)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _context.Tags
                .Include(t => t.JournalEntries)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(Tag tag)
        {
            tag.CreatedAt = DateTime.UtcNow;
            
            _context.Tags.Add(tag);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TagExists(tag.Name))
                {
                    return Conflict("Tag with this name already exists");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTag", new { id = tag.Id }, tag);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }

            _context.Entry(tag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(id))
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
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }

        private bool TagExists(string name)
        {
            return _context.Tags.Any(e => e.Name == name);
        }
    }
}