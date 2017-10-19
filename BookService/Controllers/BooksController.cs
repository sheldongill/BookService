using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BookService.Models;
using BookService.Repositories;

namespace BookService.Controllers
{
    /// <summary>
    /// Books resource endpoint.
    /// </summary>
    [Route("api/[controller]")]
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
        [Produces("application/json")]
        public IEnumerable<Book> GetAllBooks(string title = null)
        {
            if (title != null)
            {
                return _bookRepository.GetBooks().Where(b => b.BookTitle == title);
            }
            return _bookRepository.GetBooks();
        }

        /// <summary>
        /// Get book with specified id.
        /// </summary>
        /// <param name="id">ID for the book</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetBookById")]
        [Produces("application/json")]
        public IActionResult GetBookById(int id)
        {
            var book = _bookRepository.GetBook(id);
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
        public IActionResult CreateBook([FromBody]Book aBook)
        {
            try
            {
                int res = _bookRepository.AddBook(aBook);
                if (res != 0)
                {
                    return CreatedAtRoute("GetBookById", new {id = aBook.Id}, aBook);
                }
                return BadRequest(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Change Book information
        /// </summary>
        /// <param name="id">ID for the book</param>
        /// <param name="aBook">book details</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        public IActionResult UpdateBook(int id, [FromBody]Book aBook)
        {
            if (id == aBook.Id)
            {
                int res = _bookRepository.UpdateBook(id, aBook);
                if (res != 0)
                {
                    return Ok(res);
                }
                return NotFound();
            }
            return Forbid();
        }

        /// <summary>
        /// Delete a book. Why would you do that!?
        /// </summary>
        /// <param name="id">ID for the book</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var result = _bookRepository.DeleteBook(id);
            if (result != 0)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete]
        public IActionResult DeleteBook([FromBody] Book aBook)
        {
            return DeleteBookById(aBook.Id);
        }
    }
}
