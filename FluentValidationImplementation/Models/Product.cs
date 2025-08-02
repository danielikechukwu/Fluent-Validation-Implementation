using System.ComponentModel.DataAnnotations.Schema;

namespace FluentValidationImplementation.Models
{
    public class Product
    {
        public int ProductionId { get; set; }

        // Stock Keeping Unit following a specific pattern (e.g., 8 uppercase letters/digits)
        public string SKU { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        public int stock { get; set; }

        public int CategoryId { get; set; }

        public string? Description { get; set; }

        // Discount percentage (0-100)
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Discount { get; set; }

        // Manufacturing date (should not be in the future)
        public DateTime ManufacturingDate { get; set; }

        // Expiry date (must be after the manufacturing date)
        public DateTime ExpiryDate { get; set; }

        // Many-to-many relationship with Tag
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();


    }
}
