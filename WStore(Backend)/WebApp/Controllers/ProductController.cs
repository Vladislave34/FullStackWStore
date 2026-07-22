using Core.Interfaces;
using Core.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = "StoreOwner, Admin")]
public class ProductController(IProductService productService) : ControllerBase
{
    private string Lang =>
        Request.Headers["Accept-Language"].FirstOrDefault() ?? "en";

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProducts(int pageNumber = 1, int pageSize = 10)
    {
        var list = await productService.GetAllProducts(Lang,  pageNumber, pageSize);
        return Ok(list);
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductsByParams([FromQuery] Guid? categoryId, [FromQuery] Guid? genderId, [FromQuery] string? query, [FromQuery] bool? hasSale, [FromQuery] Guid? colorId, [FromQuery] Guid?  sizeId,   int pageNumber = 1, int pageSize = 10)
    {
        PageResult<ProductItemModel> list = new PageResult<ProductItemModel>();
        if (hasSale == true)
        {
            list = await productService.GetAllProducts(Lang, query, categoryId, genderId, hasSale, colorId, sizeId,  pageNumber, pageSize);
            return Ok(list);
        }
        list = await productService.GetAllProducts(Lang, query, categoryId, genderId,  colorId, sizeId, pageNumber, pageSize);
        return Ok(list);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var entity = await productService.GetProductById(id, Lang);
        return Ok(entity);
    }
    [HttpGet("{storeId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductsByStoreId(Guid storeId, [FromQuery] Guid? categoryId, [FromQuery] string? query, int pageNumber = 1, int pageSize = 10)
    {
        if (!string.IsNullOrWhiteSpace(query))
        {
            var searchResult = await productService.GetProductsByStoreId(storeId, Lang, categoryId, query, pageNumber, pageSize);
            return Ok(searchResult);        
        }
        var entity = categoryId.HasValue
            ? await productService.GetProductsByStoreId(storeId, Lang, categoryId.Value, pageNumber, pageSize)
            : await productService.GetProductsByStoreId(storeId, Lang, pageNumber, pageSize);
            
        return Ok(entity);
    }
    

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductAddUpdateModel model)
    {
        await productService.AddProduct(model);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, ProductAddUpdateModel model)
    {
        await productService.UpdateProduct(id, model);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveProduct(Guid id)
    {
        await productService.RemoveProduct(id);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveAllProducts()
    {
        await productService.RemoveAllProducts();
        return Ok();
    }
}