using System.ComponentModel;

namespace Quote_Tracker.Models

{
    public class UpdateQuote
    {
        public required int Id { get; set; }
        public string? Text { get; set; } = string.Empty;
        public string? Person { get; set; }
        public int? Chapter { get; set; }
        public int? Verse { get; set; }
        public int? Page { get; set; }
        public int? BookId { get; set; }
        public List<int> TopicIds { get; set; } = [];
    }
}