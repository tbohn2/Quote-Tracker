namespace Quote_Tracker.Models;
public class QuoteTopic
{
    public required int QuoteId { get; set; }
    public required int TopicId { get; set; }
    public Quote Quote { get; set; } = null!;
    public Topic Topic { get; set; } = null!;
}