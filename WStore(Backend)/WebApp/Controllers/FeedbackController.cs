using Core.Interfaces;
using Core.Models.Feedback;
using Core.Models.ProductVariant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "StoreOwner, Admin")]
public class FeedbackController(IFeedbackService feedbackService) : ControllerBase
{
    [HttpGet("Feedbacks/{productId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeedbacksByProduct(Guid productId)
    {
        var variants = await feedbackService.GetAllFeedbacksByProductId(productId);
        return Ok(variants);
    }

    [HttpGet("Feedback/{id}")]
    public async Task<IActionResult> GetFeedbackById(Guid id)
    {
        var variant = await feedbackService.GetFeedbackById(id);
        return Ok(variant);
    }

    [HttpPost("Add")]
    
    public async Task<IActionResult> AddFeedback([FromForm] FeedbackAddUpdateModel model)
    {
        await feedbackService.AddFeedback(model);
        return Ok();
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateFeedback(Guid id, FeedbackAddUpdateModel model)
    {
        await feedbackService.UpdateFeedback(id, model);
        return Ok();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteProductVariant(Guid id)
    {
        await feedbackService.RemoveFeedback(id);
        return Ok();
    }

    [HttpDelete("RemoveAll")]
    public async Task<IActionResult> RemoveAllFeedbacks()
    {
        await feedbackService.RemoveAllFeedbacks();
        return Ok();
    }
}