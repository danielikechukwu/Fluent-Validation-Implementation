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
                .WithMany(t => t.Products);
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Tag> Tags { get; set; }


    }
}
