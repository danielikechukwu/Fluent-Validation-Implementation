using FluentValidation;
using FluentValidationImplementation.DTOs;

namespace FluentValidationImplementation.Validators
{
    // Validator for ProductCreateDTO, which contains the rules for creating a product.
    public class ProductCreateDTOValidator : AbstractValidator<ProductCreateDTO>
    { 
        public ProductCreateDTOValidator()
        {
            // SKU validation: must not be empty and must match the specified regex pattern.
            RuleFor(p => p.SKU)
                .NotEmpty().WithMessage("SKU is required.") // Ensures SKU is provided.
                .Matches("^[A-Z0-9{8}$]").WithMessage("SKU must be 8 characters long and contain only uppercase letters and digits."); // Must be exactly 8 uppercase letters or digits.

            // Name validation: must not be empty and length must be between 3 and 50 characters.
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.") // Ensures product name is provided.
                .Length(3, 50).WithMessage("Product name must be between 3 and 50 characters."); // Validates the length of the product name.

            // Price validation: must be greater than 0 and conform to precision and scale.
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.") // Price must be positive.
                .PrecisionScale(8, 2, true).WithMessage("Price must have at most 8 digits in total and 2 decimals."); // Validates total digits and decimals (ignoring trailing zeros).

            // Stock validation: must be zero or positive.
            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative."); // Stock can't be negative.

            // CategoryId validation: must be a positive number.
            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than zero."); // Ensures a valid category is selected.

            // Description validation: if provided, must not exceed 500 characters.
            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.") // Limits description length.
                .When(p => !string.IsNullOrEmpty(p.Description)); // Applies only if description is provided.

            // Discount validation: must be between 0 and 100.
            RuleFor(p => p.Discount)
                .InclusiveBetween(0, 100).WithMessage("Discount must be between 0 and 100 percent."); // Ensures discount is a valid percentage.

            // Manufacturing date validation: must not be a future date.
            RuleFor(p => p.ManufacturingDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Manufacturing date cannot be in the future."); // Date must be in the past or today.

            // Expiry date validation: must be later than the manufacturing date.
            RuleFor(p => p.ExpiryDate)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Expiry date must be in the future or today.")
                .GreaterThan(p => p.ManufacturingDate).WithMessage("Expiry date must be after manufacturing date."); // Validates date order.

            // Tags validation: for each tag in the list, ensure it is not empty and doesn't exceed 20 characters.
            RuleForEach(p => p.Tags).ChildRules(tag =>
            {
                tag.RuleFor(t => t)
                    .NotEmpty().WithMessage("Tag cannot be empty.") // Ensures each tag is provided.
                    .MaximumLength(20).WithMessage("Tag cannot exceed 20 characters."); // Limits tag length.
            });
        }
    }
}
