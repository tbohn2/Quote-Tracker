namespace Quote_Tracker.Models
{
    public class UpdateBook
    {
        public required int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
    }
}