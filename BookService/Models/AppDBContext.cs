using Microsoft.EntityFrameworkCore;

namespace BookService.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions opts) : base(opts)
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
