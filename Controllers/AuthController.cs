using Microsoft.AspNetCore.Mvc;
using BookCatalogAPI.Models;

namespace BookCatalogAPI.Controllers
{
    [ApiController] // Indicates that this class is an API controller
    [Route("api/[controller]")] // Sets the route for this controller to "api/auth"
    public class AuthController : ControllerBase
    {
        private static List<User> Users = new List<User>(); // In-memory list to store users

        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            // Basic validation ensuring user places name and password 
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Username and password are required.");
            }

            // Check if user already exists 
            if (Users.Any(u => u.Username == user.Username))
            {
                return Conflict("User already exists.");
            }

            // Add user to in-memory list
            user.Id = Users.Count + 1;
            Users.Add(user);

            return Ok(new { Message = "Registration successful", UserId = user.Id });
        }
    }
}
