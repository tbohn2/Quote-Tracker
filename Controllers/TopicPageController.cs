using Microsoft.AspNetCore.Mvc;
using Quote_Tracker.Data;
using Quote_Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Quote_Tracker.Controllers
{
    public class TopicPageController : Controller
    {
        private readonly Quote_Tracker_Context _context;

        public TopicPageController(Quote_Tracker_Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var topics = await _context.Topics
                .OrderBy(t => t.Name)
                .ToListAsync();

            return View(topics); // Views/TopicPage/Index.cshtml
        }

        public async Task<IActionResult> Details(int id)
        {
            var topic = await _context.Topics
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
               }).FirstOrDefaultAsync(t => t.Id == id);

            if (topic == null)
                return NotFound();

            return View(topic); // Views/TopicPage/Details.cshtml
        }
    }
}
