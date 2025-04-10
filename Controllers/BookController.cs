using Quote_Tracker.Data;
using Quote_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        public async Task<IActionResult> CreateBook([FromBody] string title, string author)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest("Service name cannot be empty.");
            }

            var newBook = new Book
            {
                Title = title,
                Author = author ?? null
            };

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateBook), newBook);
        }
    }
}
