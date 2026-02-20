using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quote_Tracker.Data;
using Quote_Tracker.Models;

namespace Quote_Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private const string SessionUserIdKey = "UserId";

        private readonly Quote_Tracker_Context _context;

        public UserController(Quote_Tracker_Context context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Username and password are required.");

            var usernameExists = await _context.Users
                .AnyAsync(u => u.Username == request.Username);
            if (usernameExists)
                return Conflict("Username already taken.");

            var user = new User { Username = request.Username };
            user.SetPassword(request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateUser), new { id = user.Id }, new UserResponse { Id = user.Id, Username = user.Username });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Username and password are required.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !user.VerifyPassword(request.Password))
                return Unauthorized("Invalid username or password.");

            HttpContext.Session.SetInt32(SessionUserIdKey, user.Id);
            return Ok(new UserResponse { Id = user.Id, Username = user.Username });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var currentUserId = HttpContext.Session.GetInt32(SessionUserIdKey);
            if (currentUserId == null || currentUserId != id)
                return Unauthorized("You can only delete your own account. Log in first.");

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            HttpContext.Session.Clear();

            return NoContent();
        }
    }
}
