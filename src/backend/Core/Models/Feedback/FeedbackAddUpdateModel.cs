using Microsoft.AspNetCore.Http;

namespace Core.Models.Feedback;

public sealed record FeedbackAddUpdateModel
{
    public string Text { get; init; }
    public int Rating { get; init; }
    public Guid ProductId { get; init; }
    public List<IFormFile> Images { get; init; } = new List<IFormFile>();
}