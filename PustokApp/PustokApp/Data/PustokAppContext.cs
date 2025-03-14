using Microsoft.EntityFrameworkCore;
using PustokApp.Models;
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
        public DbSet<Setting> Setting { get; set; }
        public PustokAppContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookTag>()
                .HasKey(ps => new { ps.BookId, ps.TagId });
        }
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State==EntityState.Added )
                    entry.Entity.CreateDate = DateTime.Now;
                if (entry.State == EntityState.Modified)
                    entry.Entity.UpdateDate = DateTime.Now;

            }
            return base.SaveChanges();
        }
    }
}
