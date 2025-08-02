namespace FluentValidationImplementation.DTOs
{
    public class ProductCreateDTO
    {
        public string SKU { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int CategoryId { get; set; }

        public string? Description { get; set; }

        public decimal Discount { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        // List of tag names (will be normalized and stored in the Tags table)
        public List<string>? Tags { get; set; }
    }
}
