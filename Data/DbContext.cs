using api_lib.Models;
using Microsoft.EntityFrameworkCore;

namespace api_lib.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { 
            
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RentBook> RentedBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
           
            modelBuilder.Entity<RentBook>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Book>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<RentBook>()
                 .HasOne(rb => rb.Book)
                 .WithMany() 
                 .HasForeignKey(rb => rb.BookId);

            modelBuilder.Entity<RentBook>()
                .HasOne(rb => rb.User)
                .WithMany() 
                .HasForeignKey(rb => rb.UserId);

            base.OnModelCreating(modelBuilder);
        }


    }
}
