using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookCatalogAPI.Models;
using System.Security.Claims;

namespace BookCatalogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        // Temporary in-memory list of books
        private static List<Book> Books = new List<Book>();

        // GET: api/books → fetch all books
        [HttpGet]
        [Authorize]
        public IActionResult GetAllBooks()
        {
            var result = Books.Select(b => new
            {
                b.Id,
                b.Title,
                b.Author,
                b.Description,
                b.OwnerId
            });

            return Ok(result);
        }

        // GET: api/books/{id} → fetch single book
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetBookById(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound("Book not found.");
            }

            return Ok(book);
        }

        // POST: api/books → create new book
        [HttpPost]
        [Authorize]
        public IActionResult CreateBook([FromBody] Book newBook)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            newBook.Id = Books.Count + 1;
            newBook.OwnerId = userId;

            Books.Add(newBook);

            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }

        // PUT: api/books/{id} → update own book
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateBook(int id, [FromBody] Book updatedBook)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            if (book.OwnerId != userId)
            {
                return Forbid("You can only update your own books.");
            }

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Description = updatedBook.Description;

            return Ok(new { Message = "Book updated successfully." });
        }

        // DELETE: api/books/{id} → delete own book
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteBook(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            if (book.OwnerId != userId)
            {
                return Forbid("You can only delete your own books.");
            }

            Books.Remove(book);

            return Ok(new { Message = "Book deleted successfully." });
        }
    }
}
