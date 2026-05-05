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
    public async Task<Ok<string>> AddProduct([FromBody] AddProductDto req)
    {
        await _productService.Add(req);
        return TypedResults.Ok($"Successfully added product: {req.Name}");
    }

    [HttpGet("GetById/{id}")]
    public async Task<Ok<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetById(id);
        return TypedResults.Ok(product);
    }

    [HttpGet("GetAll")]
    public async Task<Ok<List<ProductDto>>> GetAll()
    {
        var products = await _productService.GetAll();
        return TypedResults.Ok(products);
    }

    [HttpDelete("DeleteById/{id}")]
    public async Task<Ok<string>> DeleteById(int id)
    {
        await _productService.Delete(id);
        return TypedResults.Ok($"Successfully deleted product");
    }
    
}