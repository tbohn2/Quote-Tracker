namespace Quote_Tracker.Models
{
    public class GetQuoteByBook
    {
        public required int Id { get; set; }
        public required string Text { get; set; }
        public string? Person { get; set; }
        public int? Chapter { get; set; }
        public int? Verse { get; set; }
        public int? Page { get; set; }
        public DateTime CreatedAt { get; set; }
        public required int BookId { get; set; }
        public List<GetTopic> Topics { get; set; } = new();
    }
}