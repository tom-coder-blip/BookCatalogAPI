namespace BookCatalogAPI.Models
{
    public class Book
    {
        public int Id { get; set; }                // Unique identifier
        public string Title { get; set; } = "";    // Book title
        public string Author { get; set; } = "";   // Author name
        public string Description { get; set; } = ""; // Optional summary
        public int OwnerId { get; set; }           // User who created the book
    }
}
