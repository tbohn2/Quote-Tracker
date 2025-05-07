namespace Quote_Tracker.Models

{
    public class GetTopic
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public List<GetQuoteByTopic> Quotes { get; set; } = new();
    }
}