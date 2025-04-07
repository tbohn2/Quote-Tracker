namespace Quote_Tracker.Models;
public class QuoteTopic
{
    public int QuoteId { get; set; }
    public required Quote Quote { get; set; }

    public int TopicId { get; set; }
    public required Topic Topic { get; set; }
}