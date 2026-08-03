using Core.Interfaces;
using Core.Models.Product.Adrress;
using Core.Models.Product.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]

public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCardsByUser()
    {
        var cards = await paymentService.GetCardByUser();
        return Ok(cards);
    }

    [HttpPost]
    public async Task<IActionResult> AddCard(PaymentAddUpdateModel model)
    {
        await paymentService.AddCard(model);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCard(Guid id, PaymentAddUpdateModel model)
    {
        await paymentService.UpdateCard(id, model);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCard(Guid id)
    {
        await paymentService.DeleteCard(id);
        return Ok();
    }
}