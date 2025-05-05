namespace Quote_Tracker.Models
{
    public class CreateBook
    {
        public required string Title { get; set; }
        public string? Author { get; set; }
        public required int PriorityIndex { get; set; }
    }
}