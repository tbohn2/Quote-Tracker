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
            var quotes = await _context.Quotes.ToListAsync();

            if (quotes.Count == 0)
            {
                return Ok("No quotes found");
            }

            return Ok(quotes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuote([FromBody] CreateQuoteRequest request)
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
                Book = book,
            };

            _context.Quotes.Add(newQuote);
            await _context.SaveChangesAsync();

            var quoteTopics = request.TopicIds.Select(topicId => new QuoteTopic
            {
                QuoteId = newQuote.Id,
                TopicId = topicId
            }).ToList();

            _context.Quotes.Add(newQuote);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllQuotes), new { id = newQuote.Id }, newQuote);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuote(int id, [FromBody] UpdateQuoteRequest updatedQuote)
        {
            if (updatedQuote.Id <= 0)
            {
                return BadRequest("Invalid quote ID.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Model state not valid");
            }

            var quoteToUpdate = await _context.Quotes.FindAsync(id);

            if (quoteToUpdate == null)
            {
                return NotFound("Quote not found.");
            }

            foreach (var property in typeof(Quote).GetProperties())
            {
                var newValue = property.GetValue(updatedQuote);
                if (newValue != null)
                {
                    property.SetValue(quoteToUpdate, newValue);
                }
            }

            _context.Quotes.Update(quoteToUpdate);
            await _context.SaveChangesAsync();

            return Ok(quoteToUpdate);
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
