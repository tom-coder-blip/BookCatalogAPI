using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookCatalogAPI.Models;
using System.Security.Claims;
using System.Security.Principal; // for accessing user claims in JWT token

namespace BookCatalogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // In-memory list to store users (shared with AuthController for simplicity)
        public static List<User> Users = AuthController.Users;

        // GET: api/user/{id}
        [HttpGet("{id}")]
        [Authorize] // Require JWT authentication
        public IActionResult GetUserById(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email
                // Notice: we do NOT return the password
            });
        }

        // GET: api/users 
        [HttpGet]
        [Authorize] // Require JWT authentication
        public IActionResult GetAllUsers()
        {
            var result = Users.Select(u => new
            {
                u.Id,
                u.Username,
                u.Email
            });

            return Ok(result);
        }

        // PUT: api/users/{id} → update own profile
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (userId != id)
            {
                return Forbid("You can only update your own profile.");
            }

            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Email = updatedUser.Email;
            user.Username = updatedUser.Username;

            return Ok(new { Message = "Profile updated successfully." });
        }

        // DELETE: api/users/{id} → delete own profile
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteUser(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (userId != id)
            {
                return Forbid("You can only delete your own profile.");
            }

            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            Users.Remove(user);

            return Ok(new { Message = "Profile deleted successfully." });
        }
    }
}
