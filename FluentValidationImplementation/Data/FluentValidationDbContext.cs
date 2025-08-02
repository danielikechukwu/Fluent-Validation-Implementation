using FluentValidationImplementation.Models;
using Microsoft.EntityFrameworkCore;

namespace FluentValidationImplementation.Data
{
    public class FluentValidationDbContext : DbContext
    {
        public FluentValidationDbContext(DbContextOptions<FluentValidationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship between Product and Tag using an implicit join table "ProductTag"
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Products)

            //Tells EFCore, create a table named ProductTag with TagId and ProductId as columns,
            //but don’t bother me with an explicit class.
            .UsingEntity<Dictionary<string, object>>(
                "ProductTag",
                pt => pt.HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId"),
                pt => pt.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("ProductId")
            );

            // Seed Tags (one record per line)
            modelBuilder.Entity<Tag>().HasData(
                new Tag { TagId = 1, Name = "Electronics" },
                new Tag { TagId = 2, Name = "Gaming" },
                new Tag { TagId = 3, Name = "Office" },
                new Tag { TagId = 4, Name = "Accessories" },
                new Tag { TagId = 5, Name = "Home" }
            );

            // Seed Products (one record per line)
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, SKU = "GAM12345", Name = "Gaming Laptop", Price = 1500.00m, Stock = 10, CategoryId = 1, Description = "High performance gaming laptop.", Discount = 10, ManufacturingDate = new DateTime(2023, 1, 1), ExpiryDate = new DateTime(2024, 1, 1) },
                new Product { ProductId = 2, SKU = "OFF12345", Name = "Office Desktop", Price = 800.00m, Stock = 20, CategoryId = 1, Description = "Efficient desktop for office work.", Discount = 5, ManufacturingDate = new DateTime(2023, 1, 1), ExpiryDate = new DateTime(2024, 1, 1) },
                new Product { ProductId = 3, SKU = "SMA12345", Name = "Smartphone", Price = 700.00m, Stock = 50, CategoryId = 2, Description = "Latest model smartphone.", Discount = 0, ManufacturingDate = new DateTime(2023, 1, 1), ExpiryDate = new DateTime(2024, 1, 1) },
                new Product { ProductId = 4, SKU = "WIR12345", Name = "Wireless Mouse", Price = 50.00m, Stock = 100, CategoryId = 3, Description = "Ergonomic wireless mouse.", Discount = 15, ManufacturingDate = new DateTime(2023, 1, 1), ExpiryDate = new DateTime(2024, 1, 1) },
                new Product { ProductId = 5, SKU = "MEC12345", Name = "Mechanical Keyboard", Price = 120.00m, Stock = 75, CategoryId = 3, Description = "RGB mechanical keyboard.", Discount = 20, ManufacturingDate = new DateTime(2023, 1, 1), ExpiryDate = new DateTime(2024, 1, 1) },
                new Product { ProductId = 6, SKU = "4KMON12", Name = "4K Monitor", Price = 400.00m, Stock = 30, CategoryId = 4, Description = "Ultra HD 4K monitor.", Discount = 5, ManufacturingDate = new DateTime(2023, 1, 1), ExpiryDate = new DateTime(2024, 1, 1) },
                new Product { ProductId = 7, SKU = "GAMCHAIR", Name = "Gaming Chair", Price = 300.00m, Stock = 15, CategoryId = 4, Description = "Ergonomic gaming chair.", Discount = 10, ManufacturingDate = new DateTime(2023, 1, 1), ExpiryDate = new DateTime(2024, 1, 1) },
                new Product { ProductId = 8, SKU = "BLU12345", Name = "Bluetooth Speaker", Price = 150.00m, Stock = 40, CategoryId = 5, Description = "Portable Bluetooth speaker.", Discount = 0, ManufacturingDate = new DateTime(2023, 1, 1), ExpiryDate = new DateTime(2024, 1, 1) },
                new Product { ProductId = 9, SKU = "SMAW1234", Name = "Smartwatch", Price = 250.00m, Stock = 25, CategoryId = 2, Description = "Feature-packed smartwatch.", Discount = 5, ManufacturingDate = new DateTime(2023, 1, 1), ExpiryDate = new DateTime(2024, 1, 1) },
                new Product { ProductId = 10, SKU = "HOMECAM1", Name = "Home Security Camera", Price = 100.00m, Stock = 60, CategoryId = 5, Description = "HD home security camera.", Discount = 10, ManufacturingDate = new DateTime(2023, 1, 1), ExpiryDate = new DateTime(2024, 1, 1) }
            );

            // Seed join table for ProductTag (each record provided in the same line)
            modelBuilder.Entity("ProductTag").HasData(
                new { ProductId = 1, TagId = 1 }, 
                new { ProductId = 1, TagId = 2 },
                new { ProductId = 2, TagId = 1 }, 
                new { ProductId = 2, TagId = 3 },
                new { ProductId = 3, TagId = 1 },
                new { ProductId = 4, TagId = 4 },
                new { ProductId = 5, TagId = 4 }, 
                new { ProductId = 5, TagId = 3 },
                new { ProductId = 6, TagId = 1 }, 
                new { ProductId = 6, TagId = 3 },
                new { ProductId = 7, TagId = 2 }, 
                new { ProductId = 7, TagId = 3 },
                new { ProductId = 8, TagId = 1 }, 
                new { ProductId = 8, TagId = 4 },
                new { ProductId = 9, TagId = 1 }, 
                new { ProductId = 9, TagId = 4 },
                new { ProductId = 10, TagId = 1 }, 
                new { ProductId = 10, TagId = 5 }
            );
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Tag> Tags { get; set; }


    }
}
