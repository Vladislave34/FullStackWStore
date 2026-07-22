using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;

[ApiController]
[Route("api/[controller]")]
public class SearchController(ISearchService searchService) : ControllerBase
{
    private string Lang =>
        Request.Headers["Accept-Language"].FirstOrDefault() ?? "en";

    
}