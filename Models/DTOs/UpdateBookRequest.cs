namespace Quote_Tracker.Models
{
    public class UpdateBookRequest
    {
        public required int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int? PriorityIndex { get; set; }
    }
}