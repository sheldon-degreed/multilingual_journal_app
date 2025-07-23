using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultilingualJournal.Data;
using MultilingualJournal.Models;
using MultilingualJournal.Services;

namespace MultilingualJournal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly JournalContext _context;
        private readonly IGitService _gitService;
        private readonly ILogger<TagController> _logger;

        public TagController(JournalContext context, IGitService gitService, ILogger<TagController> logger)
        {
            _context = context;
            _gitService = gitService;
            _logger = logger;
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
                
                // Create Git tag for milestone tags
                if (tag.IsMilestone)
                {
                    var gitTagCreated = await _gitService.CreateMilestoneTagAsync(
                        tag.Name, 
                        $"Milestone tag created: {tag.Name}"
                    );
                    
                    if (gitTagCreated)
                    {
                        _logger.LogInformation("Git tag created for milestone: {TagName}", tag.Name);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to create Git tag for milestone: {TagName}", tag.Name);
                    }
                }
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

            // Delete Git tag if it's a milestone
            if (tag.IsMilestone)
            {
                var gitTagDeleted = await _gitService.DeleteTagAsync(tag.Name);
                if (gitTagDeleted)
                {
                    _logger.LogInformation("Git tag deleted for milestone: {TagName}", tag.Name);
                }
                else
                {
                    _logger.LogWarning("Failed to delete Git tag for milestone: {TagName}", tag.Name);
                }
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("git-tags")]
        public async Task<ActionResult<IEnumerable<string>>> GetGitTags()
        {
            if (!_gitService.IsGitRepositoryInitialized())
            {
                return Ok(new List<string>());
            }

            var gitTags = await _gitService.GetAllTagsAsync();
            return Ok(gitTags);
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