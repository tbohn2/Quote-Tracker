using System.ComponentModel;

namespace Quote_Tracker.Models

{
    public class UpdateTopicRequest
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}