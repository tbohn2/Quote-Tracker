using System.ComponentModel;

namespace Quote_Tracker.Models

{
    public class UpdateTopic
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}