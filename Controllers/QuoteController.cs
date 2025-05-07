using Quote_Tracker.Data;
using Quote_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Quote_Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly Quote_Tracker_Context _context;

        public QuoteController(Quote_Tracker_Context context)
        {
            _context = context;
        }

        // Adjust for pagination later
        [HttpGet]
        public async Task<IActionResult> GetAllQuotes()
        {
            var quoteList = await _context.Quotes.ToListAsync();

            return Ok(quoteList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuote([FromBody] CreateQuote request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid format; check for missing information (quote or book source).");
            }

            var book = await _context.Books.FindAsync(request.BookId);

            if (book == null)
            {
                return BadRequest("Invalid BookId.");
            }

            var newQuote = new Quote
            {
                Text = request.Text,
                Person = request.Person ?? null,
                Chapter = request.Chapter ?? null,
                Verse = request.Verse ?? null,
                Page = request.Page ?? null,
                BookId = request.BookId,
            };

            _context.Quotes.Add(newQuote);
            await _context.SaveChangesAsync();

            var quoteTopics = request.TopicIds.Select(topicId => new QuoteTopic
            {
                QuoteId = newQuote.Id,
                TopicId = topicId
            }).ToList();

            _context.QuoteTopics.AddRange(quoteTopics);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateQuote([FromBody] UpdateQuote updatedQuote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state not valid");
            }

            var quoteToUpdate = await _context.Quotes.Include(q => q.QuoteTopics).FirstOrDefaultAsync(q => q.Id == updatedQuote.Id);
            if (quoteToUpdate == null)
            {
                return NotFound("Quote not found.");
            }

            foreach (var property in typeof(UpdateQuote).GetProperties())
            {
                if (property.Name == "Id") { continue; }

                var newValue = property.GetValue(updatedQuote);
                if (newValue != null)
                {
                    if (property.Name == "TopicIds" && newValue is List<int> topicIds)
                    {
                        var existingQuoteTopics = await _context.QuoteTopics
                            .Where(qt => qt.QuoteId == quoteToUpdate.Id)
                            .ToListAsync();

                        var quoteTopicsToRemove = existingQuoteTopics
                            .Where(qt => !updatedQuote.TopicIds.Contains(qt.TopicId))
                            .ToList();

                        if (quoteTopicsToRemove.Count != 0)
                        {
                            _context.QuoteTopics.RemoveRange(quoteTopicsToRemove);
                        }

                        foreach (var topicId in updatedQuote.TopicIds)
                        {
                            if (!existingQuoteTopics.Any(qt => qt.TopicId == topicId))
                            {
                                _context.QuoteTopics.Add(new QuoteTopic
                                {
                                    QuoteId = quoteToUpdate.Id,
                                    TopicId = topicId
                                });
                            }
                        }
                    }
                    else
                    {
                        var correspondingProperty = typeof(Quote).GetProperty(property.Name);

                        if (correspondingProperty != null && correspondingProperty.CanWrite)
                        {
                            correspondingProperty.SetValue(quoteToUpdate, newValue);
                        }
                    }
                }
            }

            _context.Quotes.Update(quoteToUpdate);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid quote ID.");
            }

            var quoteToDelete = await _context.Quotes.FindAsync(id);

            if (quoteToDelete == null)
            {
                return NotFound($"No quote found with ID {id}");
            }

            _context.Quotes.Remove(quoteToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
