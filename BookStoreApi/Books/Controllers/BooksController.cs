using BookStoreApi.Books.Models;
using BookStoreApi.Books.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;

        public BooksController(
            BooksService booksService) =>
            _booksService = booksService;

        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get()
        {
            var books = await _booksService.GetAsync();
            return Ok(books);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> GetById(string id)
        {
            var book = await _booksService.GetByIdAsync(id);
            if (book is null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await _booksService.CreateAsync(newBook);
            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Book updatedBook)
        {
            var book = await _booksService.GetByIdAsync(id);
            if (book is null)
            {
                return NotFound();
            }
            updatedBook.Id = book.Id;
            await _booksService.UpdateAsync(id, updatedBook);
            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _booksService.GetByIdAsync(id);
            if (book is null)
            {
                return NotFound();
            }
            await _booksService.DeleteAsync(id);
            return NoContent();
        }      
    }
}
