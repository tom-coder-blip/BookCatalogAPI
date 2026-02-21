using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookCatalogAPI.Models;

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
    }
}
