using Microsoft.AspNetCore.Http;

namespace Core.Models.Feedback;

public class FeedbackAddUpdateModel
{
    public string Text { get; set; }
    public int Rating { get; set; }
    public Guid ProductId { get; set; }
    public List<IFormFile> Images { get; set; } = new List<IFormFile>();
}