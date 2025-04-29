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
            var books = await _context.Books.ToListAsync();

            if (books.Count == 0)
            {
                return NotFound("No books found");
            }

            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request)
        {
            if (string.IsNullOrEmpty(request.Title))
            {
                return BadRequest("Title cannot be empty.");
            }

            var newBook = new Book
            {
                Title = request.Title,
                Author = request.Author
            };

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllBooks), new { id = newBook.Id }, newBook);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBookRequest updatedBook)
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

            foreach (var property in typeof(UpdateBookRequest).GetProperties())
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
