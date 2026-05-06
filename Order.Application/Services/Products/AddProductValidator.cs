using Order.Application.Dtos.Product;
using Order.Application.Interfaces.Helper;

namespace Order.Application.Services.Products;

public class AddProductValidator : IValidator<AddProductDto>
{
    public void Validate(AddProductDto req)
    {
        if (req.Price <= 0)
            throw new ArgumentException("Price must be greater than 0");
        if (req.Stock <= 0)
            throw new ArgumentException("Stock must be greater than 0");
    }
}