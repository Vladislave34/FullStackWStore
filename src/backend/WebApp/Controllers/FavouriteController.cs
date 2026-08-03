using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class FavouriteController(IFavoutiteService favoutiteService) : ControllerBase
{
    private string Lang =>
        Request.Headers["Accept-Language"].FirstOrDefault() ?? "en";
    [HttpGet]
    public async Task<IActionResult> GetFavourites()
    {
        var favourites = await favoutiteService.GetFavourites(Lang);
        return Ok(favourites);
    }

    [HttpPost("{productId}")]
    public async Task<IActionResult> AddFavourite(Guid productId)
    {
        await favoutiteService.AddFavourite(productId);
        return Ok();
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveFavourite(Guid productId)
    {
        await favoutiteService.RemoveFavourite(productId);
        return Ok();
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> IsFavourite(Guid productId)
    {
        var isFavourite = await favoutiteService.IsFavourite(productId);
        return Ok(isFavourite);
    }
    
}