namespace FluentValidationImplementation.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        // Tag name must be unique and not empty
        public string Name { get; set; }

        // Many-to-many relationship with Product
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
