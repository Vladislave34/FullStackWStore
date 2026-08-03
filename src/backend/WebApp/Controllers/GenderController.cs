
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace WebApplication1.Controlers;
[Route("api/[controller]/[action]")]
[ApiController]
[AllowAnonymous]


public class GenderController(IGenderService genderService) :  ControllerBase
{
    private string Lang =>
        Request.Headers["Accept-Language"].FirstOrDefault() ?? "en";

    [HttpGet]
    public async Task<IActionResult> GetGenders()
    {
        var genders = await genderService.GetAllGenders(Lang);
        return Ok(genders);
    }
}