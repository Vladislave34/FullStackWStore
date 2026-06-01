using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;

[ApiController]
[Route("api/[controller]")]
public class SearchController(ISearchService searchService) : ControllerBase
{
    private string Lang =>
        Request.Headers["Accept-Language"].FirstOrDefault() ?? "en";

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string query = "",
        [FromQuery] string? category = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null)
    {
        var results = await searchService.SearchAsync(query, Lang, category, minPrice, maxPrice);
        return Ok(results);
    }

    [HttpGet("autocomplete")]
    public async Task<IActionResult> Autocomplete([FromQuery] string prefix)
    {
        var results = await searchService.AutocompleteAsync(prefix, Lang);
        return Ok(results);
    }
}