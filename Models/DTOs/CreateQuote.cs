namespace Quote_Tracker.Models
{
    public class CreateQuote
    {
        public required string Text { get; set; }
        public string? Person { get; set; }
        public int? Chapter { get; set; }
        public int? Verse { get; set; }
        public int? Page { get; set; }
        public required int BookId { get; set; }
        public List<int> TopicIds { get; set; } = new();
    }
}