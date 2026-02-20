namespace Quote_Tracker.Models

{
    public class Topic
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<QuoteTopic> QuoteTopics { get; set; } = new();
    }
}