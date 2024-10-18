using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Xml;
using WebProject.Entites;

namespace WebProject.DbContextLayer
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public AppDbContext()
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Commodity> Commodities { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Specification> Specifications { get; set; }

        public DbSet<Customer> Customers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var item in builder.Model.GetEntityTypes())
            {
                var tableName = item.GetTableName();

                if (tableName.StartsWith("AspNet"))
                {
                    item.SetTableName(tableName.Substring(6));
                }
            }

            builder.Entity<Product>()
            .Property(p => p.CostPrice)
            .HasPrecision(18, 2);  // 18 là precision, 2 là scale

             builder.Entity<Product>()
             .Property(p => p.Price)
             .HasPrecision(18, 2);  // Tương tự cho trường Price

            builder.Entity<Supplier>()
            .Property(p => p.Rating)
            .HasPrecision(18, 2);  // Tương tự cho trường Price

            builder.Entity<Specification>()
           .Property(p => p.Price)
           .HasPrecision(18, 2);  // Tương tự cho trường Price

            builder.Entity<Category>(entity =>
            {
                entity.HasIndex(p => p.Slug).IsUnique();
            });

            builder.Entity<Product>(entity =>
            {
                entity.HasIndex(p => p.Slug).IsUnique();
            });

            builder.Entity<Commodity>(entity =>
            {
                entity.HasIndex(p => p.Slug).IsUnique();
            });

            builder.Entity<Order>()
            .HasOne<Specification>()
            .WithMany()
            .HasForeignKey(e => e.SpecificationID)
            .OnDelete(DeleteBehavior.Restrict); // Ngăn việc xóa khóa ngoại
        }


    }
}
