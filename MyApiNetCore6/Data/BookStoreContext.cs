using Microsoft.EntityFrameworkCore;
using MyApiNetCore6.Data;

namespace MyApiNetCore6.Data
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book>? books { set; get; }
    }
}
