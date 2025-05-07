using Quote_Tracker.Data;
using Quote_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Quote_Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly Quote_Tracker_Context _context;

        public BookController(Quote_Tracker_Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _context.Books
                .OrderBy(b => b.PriorityIndex)
                .Include(b => b.Quotes)
                .ThenInclude(q => q.QuoteTopics)
                .ThenInclude(qt => qt.Topic)
                .Select(b => new GetBook
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    PriorityIndex = b.PriorityIndex,
                    Quotes = b.Quotes.Select(q => new GetQuoteByBook
                    {
                        Id = q.Id,
                        Text = q.Text,
                        Person = q.Person,
                        Chapter = q.Chapter,
                        Verse = q.Verse,
                        Page = q.Page,
                        CreatedAt = q.CreatedAt,
                        BookId = q.BookId,
                        Topics = q.QuoteTopics.Select(qt => qt.Topic.Name).ToList()
                    }).ToList()
                })
                .ToListAsync();


            if (books.Count == 0)
            {
                return NotFound("No books found");
            }

            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBook request)
        {
            if (string.IsNullOrEmpty(request.Title))
            {
                return BadRequest("Title cannot be empty.");
            }

            var newBook = new Book
            {
                Title = request.Title,
                Author = request.Author,
                PriorityIndex = request.PriorityIndex
            };

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllBooks), new { id = newBook.Id }, newBook);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBook updatedBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state not valid");
            }

            var bookToUpdate = await _context.Books.FindAsync(updatedBook.Id);

            if (bookToUpdate == null)
            {
                return NotFound("Book not found.");
            }

            foreach (var property in typeof(UpdateBook).GetProperties())
            {
                var newValue = property.GetValue(updatedBook);
                if (newValue != null)
                {
                    var correspondingProperty = typeof(Book).GetProperty(property.Name);
                    if (correspondingProperty != null && correspondingProperty.CanWrite)
                    {
                        correspondingProperty.SetValue(bookToUpdate, newValue);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok(bookToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid book ID.");
            }

            var bookToDelete = await _context.Books.FindAsync(id);

            if (bookToDelete == null)
            {
                return NotFound($"No book found with ID {id}");
            }

            _context.Books.Remove(bookToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
