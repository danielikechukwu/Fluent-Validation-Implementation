namespace FluentValidationImplementation.DTOs
{
    public class ProductResponseDTO
    {
        public int ProductId { get; set; }

        public string SKU { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int CategoryId { get; set; } 

        public string? Description { get; set; }

        public decimal Discount { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public List<string>? Tags { get; set; }
    }
}
