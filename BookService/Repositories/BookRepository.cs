using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookService.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Serilog;

namespace BookService.Repositories
{
    public interface IBookRepository<TEntity, U> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetBooks();
        Task<TEntity> GetBook(U id);
        Task<int> AddBook(TEntity b);
        Task<int> UpdateBook(U id, TEntity b);
        Task<int> RemoveBook(U id);
    }

    public class BookRepository : IBookRepository<Book, int>
    {
        AppDbContext dbContext;
        readonly ILogger logger = Log.ForContext<BookRepository>();

        public BookRepository(AppDbContext context)
        {
            dbContext = context;
        }

        public async Task<int> AddBook(Book b)
        {
            // FIXME: Should check that b.Id isn't already used.
            dbContext.Books.Add(b);
            int res = await dbContext.SaveChangesAsync();
            if (res != 0)
            {
                logger.Debug("Added Book: {@Book}",b);
            }
            return res;
        }

        public async Task<int> RemoveBook(int id)
        {
            int res = 0;
            var book = dbContext.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                dbContext.Books.Remove(book);
                res = await dbContext.SaveChangesAsync();
            }
            if (res != 0)
            {
                logger.Debug("Deleted Book: {@Book}", book);
            }
            return res;
        }

        public async Task<Book> GetBook(int id)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            return book;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await dbContext.Books.ToListAsync();
        }

        public async Task<int> UpdateBook(int id, Book b)
        {
            int res = 0;
            var book = dbContext.Books.Find(id);
            if ((book != null) && (b.Id == id))
            {
                Book changedBook = new Book();  // this is so we can log only the changed fields.

                if (book.BookTitle != b.BookTitle)
                    changedBook.BookTitle = b.BookTitle;
                if (book.AuthorName != b.AuthorName)
                    changedBook.AuthorName = b.AuthorName;
                if (book.Publisher != b.Publisher)
                    changedBook.Publisher = b.Publisher;
                if (book.Genre != b.Genre)
                    changedBook.Genre = b.Genre;

                book.BookTitle = b.BookTitle;
                book.AuthorName = b.AuthorName;
                book.Publisher = b.Publisher;
                book.Genre = b.Genre;
                book.Price = b.Price;
                res = await dbContext.SaveChangesAsync();
                if (res != 0)
                {
                    logger.Debug("Altered {@Book}", changedBook);
                }
            }
            return res;
        }
    }
}
