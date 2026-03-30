using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;

[ApiController]
[Route("api/[controller]")]
public class SearchController(ISearchService searchService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string query,
        [FromQuery] string? category,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        var results = await searchService.SearchAsync(query, category, minPrice, maxPrice);
        return Ok(results);
    }

    [HttpGet("autocomplete")]
    public async Task<IActionResult> Autocomplete([FromQuery] string prefix)
    {
        var results = await searchService.AutocompleteAsync(prefix);
        return Ok(results);
    }
}