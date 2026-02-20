using Microsoft.AspNetCore.Mvc; // Provides attributes and classes for building API controllers
using Microsoft.IdentityModel.Tokens; // Provides classes for handling JWT tokens and security
using System.IdentityModel.Tokens.Jwt; // Provides classes for creating and validating JWT tokens
using System.Security.Claims; // Provides classes for handling claims-based identity, which is used in JWT tokens to represent user information and permissions
using System.Text; // Provides classes for encoding and decoding text, which is used for handling the secret key in JWT token generation and validation
using BookCatalogAPI.Models; // for models

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

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            var user = Users.FirstOrDefault(u => u.Username == loginUser.Username && u.Password == loginUser.Password);

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Create JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("SuperSecretKey12345_SuperSecretKey12345");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(new { Token = jwt });
        }
    }
}
