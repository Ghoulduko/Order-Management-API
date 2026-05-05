using Order.Application.Dtos.Product;

namespace Order.Application.Interfaces;

public interface IProductService
{
    Task Add(AddProductDto request);
    Task<ProductDto> GetById(int id);
    Task<List<ProductDto>> GetAll();
    Task DecreaseStock(int productId, int quantity);
    Task Delete(int id);
}