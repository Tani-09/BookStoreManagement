using BookStoreManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
namespace BookStoreManagement.DAL.DBContext

{

    public class BookStoreDbContext : DbContext
    {
        // Constructor that takes DbContextOptions as parameter
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
        {
        }

        // Define DbSets for each entity (these are your tables)
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        // Optional: Customize the model creation (e.g., relationships, table names)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)  // A User has one Role
                .WithMany()  // A Role can have many Users
                .HasForeignKey(u => u.RoleId);  // Foreign key is RoleId

            modelBuilder.Entity<Book>()
        .Property(b => b.Price)
        .HasColumnType("decimal(18,2)"); // Precision 18, scale 2 (you can adjust as needed)

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired(); // Precision 18, scale 2 (you can adjust as needed)

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)"); // Precision 18, scale 2 (you can adjust as needed)

            // Example: You can customize table names, relationships, etc.
            // modelBuilder.Entity<Book>().ToTable("Books"); 
            // This is just an example, you can add customizations as needed

            SeedRoles(modelBuilder);
        }

        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                
                new Role { Id = 3, Name = "Guest" } // Add the Guest role
            );
        }
    }
}