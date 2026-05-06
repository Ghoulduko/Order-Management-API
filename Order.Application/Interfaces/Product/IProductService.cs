using Order.Application.Dtos.Product;

namespace Order.Application.Interfaces;

public interface IProductService
{
    Task Add(AddProductDto request);
    Task<List<ProductDto>> GetAll();
    Task<List<ProductDto>> SearchProductByName(string name);
    Task<ProductDto> GetById(int id);
    Task IncreaseStock(UpdateProductStockDto req);
    Task Delete(int id);
}