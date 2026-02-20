using Quote_Tracker.Data;
using Quote_Tracker.Models;
using Quote_Tracker.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Quote_Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RequireAuth]
    public class TopicController : ControllerBase
    {
        private readonly Quote_Tracker_Context _context;

        public TopicController(Quote_Tracker_Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopics()
        {
            var userId = CurrentUser.GetUserId(HttpContext)!.Value;
            var topics = await _context.Topics
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.Name)
                .Include(t => t.QuoteTopics)
                    .ThenInclude(qt => qt.Quote)
                        .ThenInclude(q => q.Book)
                .Select(t => new GetTopic
                {
                    Id = t.Id,
                    Name = t.Name,
                    Quotes = t.QuoteTopics.Select(qt => new GetQuoteByTopic
                    {
                        Id = qt.Quote.Id,
                        Text = qt.Quote.Text,
                        Person = qt.Quote.Person,
                        Chapter = qt.Quote.Chapter,
                        Verse = qt.Quote.Verse,
                        CreatedAt = qt.Quote.CreatedAt,
                        BookId = qt.Quote.Book.Id,
                        BookTitle = qt.Quote.Book.Title,
                        BookAuthor = qt.Quote.Book.Author,
                        BookPriorityIndex = qt.Quote.Book.PriorityIndex
                    })
                    .OrderBy(q => q.BookPriorityIndex)
                    .ThenBy(q => q.Chapter ?? int.MaxValue)
                    .ThenBy(q => q.Verse ?? int.MaxValue)
                    .ToList()
                }).ToListAsync();

            return Ok(topics);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopic request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid format; check for missing information (Topic or book source).");

            var userId = CurrentUser.GetUserId(HttpContext)!.Value;
            var newTopic = new Topic
            {
                Name = request.Name,
                UserId = userId
            };

            _context.Topics.Add(newTopic);
            await _context.SaveChangesAsync();

            var topics = await _context.Topics
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.Name)
                .ToListAsync();

            return CreatedAtAction(nameof(GetAllTopics), new { id = newTopic.Id }, topics);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTopic([FromBody] UpdateTopic updatedTopic)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model state not valid");

            var userId = CurrentUser.GetUserId(HttpContext)!.Value;
            var topicToUpdate = await _context.Topics.FindAsync(updatedTopic.Id);
            if (topicToUpdate == null)
                return NotFound("Topic not found.");
            if (topicToUpdate.UserId != userId)
                return Forbid();

            topicToUpdate.Name = updatedTopic.Name;
            _context.Topics.Update(topicToUpdate);
            await _context.SaveChangesAsync();

            return Ok(topicToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Topic ID.");

            var userId = CurrentUser.GetUserId(HttpContext)!.Value;
            var topicToDelete = await _context.Topics.FindAsync(id);
            if (topicToDelete == null)
                return NotFound($"No Topic found with ID {id}");
            if (topicToDelete.UserId != userId)
                return Forbid();

            _context.Topics.Remove(topicToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
