using Order.Core.Entities;
using Order.Core.Exceptions;
using Order.Core.Interfaces;

namespace Order.Application.Services.Products;

public class InventoryService
{
    private readonly IGenericRepository<Product> _repository;

    public InventoryService(IGenericRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task IncreaseStock(int productId, int quantity)
    {
        var product = await _repository.GetByIdAsync(productId);
        if (product == null)
            throw new NotFoundException("Product not found");
        product.Stock += quantity;
        await _repository.SaveAsync();
    }
    
    public async Task DecrementStock(int productId, int quantity)
    {
        var product = await _repository.GetByIdAsync(productId);
        if (product == null)
            throw new NotFoundException("Product not found");
        product.Stock -= quantity;
        await _repository.SaveAsync();
    }
}