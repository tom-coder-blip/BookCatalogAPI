namespace BookCatalogAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // later: hash this
        public string Email { get; set; } = string.Empty;
    }
}
