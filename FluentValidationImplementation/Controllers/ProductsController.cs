using FluentValidationImplementation.Data;
using FluentValidationImplementation.DTOs;
using FluentValidationImplementation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography.X509Certificates;

namespace FluentValidationImplementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // The database context used to interact with the underlying database.
        private readonly FluentValidationDbContext _context;

        private readonly ILogger<ProductsController> _logger;

        // Constructor that initializes the database context.
        public ProductsController(FluentValidationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/products?tags=tag1,tag2
        // Retrieves all products, optionally filtered by any matching tags.
        [HttpGet]
        public async Task<ActionResult<ProductResponseDTO>> GetProducts([FromQuery] string? tags)
        {
            // Build the query and include the related Tags collection for each product.
            IQueryable<Product> query = _context.Products
                .AsNoTracking()
                .Include(p => p.Tags);

            if (!string.IsNullOrEmpty(tags))
            {
                // Split the comma-separated tags string into a list.
                // Trim spaces and convert each tag to lower case for case-insensitive comparison.
                var tagList = tags.Split(',')
                    .Select(t => t.Trim().ToLower())
                    .ToList();

                _logger.LogInformation("Tag list: {tagList}", tagList);

                // Filter products that contain ANY of the specified tags.
                // If a product has at least one matching tag, it is included in the result.
                query = query.Where(p => p.Tags.Any(t => tagList.Contains(t.Name.ToLower())));

            }

            // Execute the query and retrieve the list of products from the database.
            var products = await query.ToListAsync();

            // Map each Product entity to a ProductResponseDTO.
            var result = products.Select(p => new ProductResponseDTO
            {
                ProductId = p.ProductId,
                SKU = p.SKU,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                Description = p.Description,
                Discount = p.Discount,
                ManufacturingDate = p.ManufacturingDate,
                ExpiryDate = p.ExpiryDate,
                // Map the Tags collection to a list of tag names.
                Tags = p.Tags.Select(t => t.Name).ToList()
            }).ToList();

            // Return the filtered list of products with an HTTP 200 OK status.
            return Ok(result);
        }

        // GET: api/products/{id}
        // Retrieves a single product by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProduct([FromRoute] int id)
        {
            // Retrieve the product with the given ID, including its Tags.
            // AsNoTracking() is used here since no update is needed (improves performance).
            // Find the product by ID, including its related Tags collection.
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            // If the product is not found, return a 404 Not Found response.
            if (product == null)
            {
                return NotFound();
            }

            // Map the Product entity to a ProductResponseDTO.
            var result = new ProductResponseDTO
            {
                ProductId = product.ProductId,
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Discount = product.Discount,
                ManufacturingDate = product.ManufacturingDate,
                ExpiryDate = product.ExpiryDate,
                // Map the Tags collection to a list of tag names.
                Tags = product.Tags.Select(t => t.Name).ToList()
            };

            // Return the found product with an HTTP 200 OK status.
            return Ok(result);

        }

        // POST: api/products
        // Creates a new product based on the data provided in ProductCreateDTO.
        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> CreateProduct([FromBody] ProductCreateDTO productCreateDto)
        {
            // Validate the incoming DTO; if invalid, return a 400 Bad Request with the validation errors.
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the incoming DTO to a new Product entity.
            var product = new Product
            {
                SKU = productCreateDto.SKU,
                Name = productCreateDto.Name,
                Price = productCreateDto.Price,
                Stock = productCreateDto.Stock,
                CategoryId = productCreateDto.CategoryId,
                Description = productCreateDto.Description,
                Discount = productCreateDto.Discount,
                ManufacturingDate = productCreateDto.ManufacturingDate,
                ExpiryDate = productCreateDto.ExpiryDate
            };

            // Process Tags: For each tag provided in the DTO, check if it exists.
            // If the tag exists, use the existing Tag; otherwise, create a new Tag.
            if(productCreateDto.Tags != null && productCreateDto.Tags.Any())
            {
                foreach(var tagName in productCreateDto.Tags)
                {
                    // Normalize the tag by trimming whitespace and converting to lower case.
                    var normalizeTagName = tagName.Trim().ToLower();

                    var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == normalizeTagName);

                    if (existingTag != null)
                    {
                        product.Tags.Add(existingTag);
                    }
                    else
                    {
                        product.Tags.Add(new Tag { Name = normalizeTagName });  // Create a new tag and associate it.
                    }
                }
            }

            // Add the new product to the database context.
            _context.Products.Add(product);

            // Save changes to persist the new product (and associated tags) to the database.
            await _context.SaveChangesAsync();

            // Map the newly created Product entity to a ProductResponseDTO.
            var response = new ProductResponseDTO
            {
                ProductId = product.ProductId,
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Discount = product.Discount,
                ManufacturingDate = product.ManufacturingDate,
                ExpiryDate = product.ExpiryDate,
                Tags = product.Tags.Select(t => t.Name).ToList()
            };

            // Return the created product with an HTTP 200 OK (or 201 Created) status.
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, response);
        }
    }
}
