using System.Collections.Generic;
using System.Linq;
using BookService.Models;
using Serilog;

namespace BookService.Repositories
{
    public interface IBookRepository<TEntity, U> where TEntity : class
    {
        IEnumerable<TEntity> GetBooks();
        TEntity GetBook(U id);
        int AddBook(TEntity b);
        int UpdateBook(U id, TEntity b);
        int DeleteBook(U id);
    }

    public class BookRepository : IBookRepository<Book, int>
    {
        AppDBContext ctx;
        readonly ILogger logger = Log.ForContext<BookRepository>();

        public BookRepository(AppDBContext c)
        {
            ctx = c;
        }
        public int AddBook(Book b)
        {
            // FIXME: Should check that b.Id isn't already used.
            ctx.Books.Add(b);
            int res = ctx.SaveChanges();
            if (res != 0)
            {
                logger.Debug("Added Book: {Book}",b);
            }
            return res;
        }

        public int DeleteBook(int id)
        {
            int res = 0;
            var book = ctx.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                ctx.Books.Remove(book);
                res = ctx.SaveChanges();
            }
            return res;
        }

        public Book GetBook(int id)
        {
            var book = ctx.Books.FirstOrDefault(b => b.Id == id);
            return book;
        }

        public IEnumerable<Book> GetBooks()
        {
            var books = ctx.Books.ToList();
            return books;
        }

        public int UpdateBook(int id, Book b)
        {
            int res = 0;
            var book = ctx.Books.Find(id);
            if (book != null)
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
                res = ctx.SaveChanges();
                if (res != 0)
                {
                    logger.Debug("Altered {@Book}", changedBook);
                }
            }
            return res;
        }
    }
}
