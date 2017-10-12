using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BookService.Models;
using BookService.Repositories;
using SQLitePCL;

namespace BookService.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private IBookRepository<Book, int> _bookRepository;

        public BooksController(IBookRepository<Book, int> bRepository)
        {
            _bookRepository = bRepository;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            return _bookRepository.GetBooks();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetBookById")]
        public IActionResult Get(int id)
        {
            var book = _bookRepository.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Book aBook)
        {
            try
            {
                int res = _bookRepository.AddBook(aBook);
                if (res != 0)
                {
                    return Ok(res);
                    //return CreatedAtRoute("GetBookById", res);
                }
                return BadRequest(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return BadRequest();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Book aBook)
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

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _bookRepository.DeleteBook(id);
            if (result != 0)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
