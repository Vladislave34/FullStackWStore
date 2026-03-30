using Domain.Entities.Identity;

namespace Core.Models.Feedback;

public class FeedbackItemModel
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public int Rating { get; set; }
    public Guid UserId { get; set; }
    public List<string> Images { get; set; } = new List<string>();
}