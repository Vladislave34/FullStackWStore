using Core.Interfaces;
using Core.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "StoreOwner, Admin")]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet("Products")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProducts()
    {
        var list = await productService.GetAllProducts();
        return Ok(list);
    }

    [HttpGet("Product/{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var entity = await productService.GetProductById(id);
        return Ok(entity);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddProduct(ProductAddUpdateModel model)
    {
        await productService.AddProduct(model);
        return Ok();
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, ProductAddUpdateModel model)
    {
        await productService.UpdateProduct(id, model);
        return Ok();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> RemoveProduct(Guid id)
    {
        await productService.RemoveProduct(id);
        return Ok();
    }
    [HttpDelete("RemoveAll")]
    public async Task<IActionResult> RemoveAllProducts()
    {
        await productService.RemoveAllProducts();
        return Ok();
    }
}