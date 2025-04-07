namespace Quote_Tracker.Models

{
    public class Topic
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<QuoteTopic> QuoteTopics { get; set; } = new();
    }
}