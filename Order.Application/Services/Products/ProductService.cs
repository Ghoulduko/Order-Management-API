using AutoMapper;
using Order.Application.Dtos.Product;
using Order.Application.Interfaces;
using Order.Core.Entities;
using Order.Core.Interfaces;

namespace Order.Application.Services.Products;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _repository;
    private readonly IMapper _mapper;
    
    public ProductService(IGenericRepository<Product> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task Add(AddProductDto req)
    {
        if (req.Price <= 0)
            throw new ArgumentException("Price must be greater than 0");
        if (req.Stock <= 0)
            throw new ArgumentException("Stock must be greater than 0");
        
        var products = await _repository.FilterAsync(p => p.Name == req.Name);
        var productInDb = products.FirstOrDefault();
        
        if (productInDb != null)
        {
            productInDb.Stock += req.Stock;
            await _repository.SaveAsync();
            return;
        }

        var product = _mapper.Map<Product>(req);
        await _repository.AddAsync(product);
    }

    public async Task<ProductDto> GetById(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<List<ProductDto>> GetAll()
    {
        var products = await _repository.GetAllAsync();
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task DecreaseStock(int productId, int quantity)
    {
        var product = await _repository.GetByIdAsync(productId);
        product.Stock -= quantity;
    }

    public async Task Delete(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(product);
    }
}