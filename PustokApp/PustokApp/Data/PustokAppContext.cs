using Microsoft.EntityFrameworkCore;
using PustokApp.Models.Home;

namespace PustokApp.Data
{
    public class PustokAppContext : DbContext
    {
        public DbSet<Slider> Slider { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<BookImage> BookImage { get; set; }
        public DbSet<BookTag> BookTag { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Feature> Feature { get; set; }
        public PustokAppContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookTag>()
                .HasKey(ps => new { ps.BookId, ps.TagId });
        }
    }
}
