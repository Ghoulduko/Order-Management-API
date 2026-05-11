using AutoMapper;
using Order.Application.Dtos.Product;
using Order.Application.Interfaces;
using Order.Application.Interfaces.Helper;
using Order.Core.Entities;
using Order.Core.Exceptions;
using Order.Core.Interfaces;

namespace Order.Application.Services.Products;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _repository;
    private readonly IValidator<AddProductDto> _validator;
    private readonly InventoryService _inventoryService;
    private readonly IMapper _mapper;
    
    public ProductService(IGenericRepository<Product> repository, IValidator<AddProductDto> validator, InventoryService inventoryService, IMapper mapper)
    {
        _repository = repository;
        _validator = validator;
        _inventoryService = inventoryService;
        _mapper = mapper;
    }
    
    public async Task Add(AddProductDto req)
    {
        _validator.Validate(req); 
        
        var productInDb = await _repository.GetSingleOrDefaultAsync(p => p.Name == req.Name);
        
        if (productInDb != null)
        {
            productInDb.Stock += req.Stock;
            await _repository.SaveAsync();
            return;
        }

        var product = _mapper.Map<Product>(req);
        await _repository.AddAsync(product);
    }

    public async Task<List<ProductDto>> GetAll()
    {
        var products = await _repository.GetAllAsync();
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<List<ProductDto>> SearchProductByName(string name)
    {
        var products = await _repository.FilterAsync(p => p.Name.Contains(name));
        if (!products.Any())
        {
            var allProducts = await _repository.GetAllAsync();
            return _mapper.Map<List<ProductDto>>(allProducts);
        }
        return _mapper.Map<List<ProductDto>>(products);
    }
    
    public async Task<ProductDto> GetById(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
            throw new NotFoundException("Product not found");
        return _mapper.Map<ProductDto>(product);
    }

    public async Task IncreaseStock(UpdateProductStockDto req)
    {
        await _inventoryService.IncreaseStock(req.productId, req.quantity);
    }

    public async Task Delete(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
            throw new NotFoundException("Product not found");
        await _repository.DeleteAsync(product);
    }
}