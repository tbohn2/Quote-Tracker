namespace Quote_Tracker.Models

{
    public class GetBook
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Author { get; set; }
        public required int PriorityIndex { get; set; }
        public ICollection<GetQuoteByBook> Quotes { get; set; } = new List<GetQuoteByBook>();
    }
}