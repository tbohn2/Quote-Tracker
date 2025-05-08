using Microsoft.AspNetCore.Mvc;
using Quote_Tracker.Data;
using Microsoft.EntityFrameworkCore;

namespace Quote_Tracker.Controllers
{
    public class BookPageController : Controller
    {
        private readonly Quote_Tracker_Context _context;

        public BookPageController(Quote_Tracker_Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books
                .OrderBy(b => b.PriorityIndex)
                .ToListAsync();

            return View(books); // Views/BookPage/Index.cshtml
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.Quotes)
                .ThenInclude(q => q.QuoteTopics)
                .ThenInclude(qt => qt.Topic)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            return View(book); // Views/BookPage/Details.cshtml
        }
    }
}
