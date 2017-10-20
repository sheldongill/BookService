using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookService.Models;
using BookService.Repositories;
using BookService.ControllerExtensions;
using Microsoft.AspNetCore.Authorization;

namespace BookService.Controllers
{
    /// <summary>
    /// Books resource endpoint.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BooksController : Controller
    {
        private IBookRepository<Book, int> _bookRepository;

        public BooksController(IBookRepository<Book, int> bRepository)
        {
            _bookRepository = bRepository;
        }

        /// <summary>
        ///   Get ALL the books.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Book>), 200)]
        public async Task<IEnumerable<Book>> GetAllBooks(string title = null)
        {
            if (title != null)
            {
                var books = await _bookRepository.GetBooks();
                return books.Where(b => b.BookTitle == title);
            }
            return await _bookRepository.GetBooks();
        }

        /// <summary>
        /// Get book with specified id.
        /// </summary>
        /// <param name="id">ID for the book</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetBookById")]
        [ProducesResponseType(typeof(Book), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookRepository.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        /// <summary>
        /// Create a new book
        /// </summary>
        /// <param name="aBook">details about the book</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Book), 201)]
        [ProducesResponseType(typeof(WebAPIError), 400)]
        public async Task<IActionResult> CreateBook([FromBody]Book aBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                int res = await _bookRepository.AddBook(aBook);
                if (res != 0)
                {
                    return CreatedAtRoute("GetBookById", new {id = aBook.Id}, aBook);
                }
                return BadRequest(new WebAPIError("Failed to create book."));
            }
            catch (Exception ex)
            {
                return BadRequest(new WebAPIError(ex.Message, "book-exception"));
            }
        }

        /// <summary>
        /// Change Book information
        /// </summary>
        /// <param name="id">ID for the book</param>
        /// <param name="aBook">book details</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(WebAPIError), 400)]
        [ProducesResponseType(typeof(WebAPIError), 409)]
        public async Task<IActionResult> UpdateBook(int id, [FromBody]Book aBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id == aBook.Id)
            {
                int res = await _bookRepository.UpdateBook(id, aBook);
                if (res != 0)
                {
                    return Ok();
                }
                return NotFound(new WebAPIError("Failed to update book."));
            }
            return this.Conflict(new WebAPIError("Resource IDs do not match."));
        }

        /// <summary>
        /// Delete a book. Why would you do that!?
        /// </summary>
        /// <param name="id">ID for the book</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookById(int id)
        {
            var result = await _bookRepository.RemoveBook(id);
            if (result != 0)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
