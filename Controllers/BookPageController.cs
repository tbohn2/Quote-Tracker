using Microsoft.AspNetCore.Mvc;
using Quote_Tracker.Data;
using Quote_Tracker.Filters;
using Microsoft.EntityFrameworkCore;

namespace Quote_Tracker.Controllers
{
    [RequireAuth]
    public class BookPageController : Controller
    {
        private readonly Quote_Tracker_Context _context;

        public BookPageController(Quote_Tracker_Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = CurrentUser.GetUserId(HttpContext)!.Value;
            var books = await _context.Books
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.PriorityIndex)
                .ToListAsync();

            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = CurrentUser.GetUserId(HttpContext)!.Value;
            var book = await _context.Books
                .Include(b => b.Quotes)
                .ThenInclude(q => q.QuoteTopics)
                .ThenInclude(qt => qt.Topic)
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (book == null)
                return NotFound();

            return View(book);
        }
    }
}
