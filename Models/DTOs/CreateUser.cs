using System.ComponentModel.DataAnnotations;

namespace Quote_Tracker.Models
{
    public class CreateUser
    {
        [Required]
        [MinLength(1)]
        public required string Username { get; set; }

        [Required]
        [MinLength(1)]
        public required string Password { get; set; }
    }
}
