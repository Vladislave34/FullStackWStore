using Core.Interfaces;

namespace Core.Models.ProductVariantImage;

public class FeedbackImageItemModel : IImageItemModel
{
    public Guid Id { get; set; }
    public string Path { get; set; }

    
    public Guid FeedbackId { get; set; }
}