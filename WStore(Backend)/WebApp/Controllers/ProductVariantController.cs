using Core.Interfaces;
using Core.Models.ProductVariant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "StoreOwner, Admin")]
public class ProductVariantController(IProductVariantServices productVariantServices) : ControllerBase
{
    [HttpGet("ProductVariants/{productId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductVariants(Guid productId)
    {
        var variants = await productVariantServices.GetAllProductVariantsByProductId(productId);
        return Ok(variants);
    }

    [HttpGet("ProductVariant/{id}")]
    public async Task<IActionResult> GetProductVariantById(Guid id)
    {
        var variant = await productVariantServices.GetProductVariantById(id);
        return Ok(variant);
    }

    [HttpPost("Add")]
    
    public async Task<IActionResult> AddProductVariant([FromForm] ProductVariantAddUpdateModel model)
    {
        await productVariantServices.AddProductVariant(model);
        return Ok();
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateProductVariant(Guid id, ProductVariantAddUpdateModel model)
    {
        await productVariantServices.UpdateProductVariant(id, model);
        return Ok();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteProductVariant(Guid id)
    {
        await productVariantServices.RemoveProductVariant(id);
        return Ok();
    }

    [HttpDelete("RemoveAll")]
    public async Task<IActionResult> RemoveAllProductVariants()
    {
        await productVariantServices.RemoveAllProductVariants();
        return Ok();
    }
}