using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Dtos.Payment;
using Order.Application.Dtos.Product;
using Order.Application.Interfaces;

namespace Order_Management_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("AddProduct")]
    [Authorize(Roles = "OWNER,INVENTORY_MANAGER")]
    public async Task<Ok<string>> AddProduct([FromBody] AddProductDto req)
    {
        await _productService.Add(req);
        return TypedResults.Ok($"Successfully added product: {req.Name}");
    }

    [HttpPatch("IncreaseStock")]
    [Authorize(Roles = "OWNER,INVENTORY_MANAGER")]
    public async Task<Ok<string>> IncreaseStock([FromBody] UpdateProductStockDto req)
    {
        await _productService.IncreaseStock(req);
        return TypedResults.Ok($"Successfully increased product stock by: {req.quantity}");
    }
    
    [HttpGet("GetAll")]
    public async Task<Ok<List<ProductDto>>> GetAll()
    {
        var products = await _productService.GetAll();
        return TypedResults.Ok(products);
    }

    [HttpGet("SearchProductByName")]
    public async Task<Ok<List<ProductDto>>> SearchProductByName([FromQuery] string? name)
    {
        return TypedResults.Ok(await _productService.SearchProductByName(name));
    }
    
    [HttpGet("GetById/{id}")]
    public async Task<Ok<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetById(id);
        return TypedResults.Ok(product);
    }

    [HttpDelete("DeleteById/{id}")]
    [Authorize(Roles = "OWNER,INVENTORY_MANAGER")]
    public async Task<Ok<string>> DeleteById(int id)
    {
        await _productService.Delete(id);
        return TypedResults.Ok($"Successfully deleted product");
    }
    
}