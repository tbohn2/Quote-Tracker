namespace Quote_Tracker.Models

{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Author { get; set; }
        public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
    }
}