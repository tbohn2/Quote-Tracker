namespace Quote_Tracker.Models;
public class QuoteTopic
{
    public required int QuoteId { get; set; }
    public required int TopicId { get; set; }
    public virtual Quote Quote { get; set; }
    public virtual Topic Topic { get; set; }
}