using Microsoft.AspNetCore.Mvc;
using Quote_Tracker.Data;
using Quote_Tracker.Models;
using Quote_Tracker.Filters;
using Microsoft.EntityFrameworkCore;

namespace Quote_Tracker.Controllers
{
    [RequireAuth]
    public class TopicPageController : Controller
    {
        private readonly Quote_Tracker_Context _context;

        public TopicPageController(Quote_Tracker_Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = CurrentUser.GetUserId(HttpContext)!.Value;
            var topics = await _context.Topics
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.Name)
                .ToListAsync();

            return View(topics);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = CurrentUser.GetUserId(HttpContext)!.Value;
            var topic = await _context.Topics
                .Where(t => t.Id == id && t.UserId == userId)
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
                }).FirstOrDefaultAsync();

            if (topic == null)
                return NotFound();

            return View(topic);
        }
    }
}
