using Core.Interfaces;
using Core.Models.Feedback;
using Core.Models.ProductVariant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class FeedbackController(IFeedbackService feedbackService) : ControllerBase
{
    [HttpGet("{productId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeedbacksByProduct(Guid productId)
    {
        var variants = await feedbackService.GetAllFeedbacksByProductId(productId);
        return Ok(variants);
    }

    [HttpGet("{id}")]
   
    public async Task<IActionResult> GetFeedbackById(Guid id)
    {
        var variant = await feedbackService.GetFeedbackById(id);
        return Ok(variant);
    }

    [HttpPost]
    
    public async Task<IActionResult> AddFeedback([FromForm] FeedbackAddUpdateModel model)
    {
        await feedbackService.AddFeedback(model);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFeedback(Guid id, FeedbackAddUpdateModel model)
    {
        await feedbackService.UpdateFeedback(id, model);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeedback(Guid id)
    {
        await feedbackService.RemoveFeedback(id);
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveAllFeedbacks()
    {
        await feedbackService.RemoveAllFeedbacks();
        return Ok();
    }
}