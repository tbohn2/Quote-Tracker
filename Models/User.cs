using BCrypt.Net;

namespace Quote_Tracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public string Password { get; private set; } = null!;

        public List<Book> Books { get; set; } = new();
        public List<Topic> Topics { get; set; } = new();

        public void SetPassword(string plainPassword)
        {
            Password = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public bool VerifyPassword(string plainPassword) => BCrypt.Net.BCrypt.Verify(plainPassword, Password);
    }
}
