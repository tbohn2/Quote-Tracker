namespace Quote_Tracker.Models

{
    public class GetBookRequest
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Author { get; set; }
    }
}