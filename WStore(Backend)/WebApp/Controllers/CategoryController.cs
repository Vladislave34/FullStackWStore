using Core.Interfaces;
using Core.Models.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]")]
[ApiController]

[Authorize(Roles = "StoreOwner, Admin")]
public class CategoryController(ICategoryService categoryService): ControllerBase
{
    private string Lang =>
        Request.Headers["Accept-Language"].FirstOrDefault() ?? "en";

    [HttpGet("Categories")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var list = await categoryService.GetAllCategories(Lang);
        return Ok(list);
    }

    [HttpGet("Category/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var entity = await categoryService.GetCategoryById(id, Lang);
        return Ok(entity);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddCategory([FromForm] CategoryAddUpdateModel model)
    {
        await categoryService.AddCategory(model);
        return Ok();
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromForm] CategoryAddUpdateModel model)
    {
        await categoryService.UpdateCategory(id, model);
        return Ok();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> RemoveCategory(Guid id)
    {
        await categoryService.RemoveCategory(id);
        return Ok();
    }

    [HttpDelete("RemoveAll")]
    public async Task<IActionResult> RemoveAllCategories()
    {
        await categoryService.RemoveAllCategories();
        return Ok();
    }
    

}