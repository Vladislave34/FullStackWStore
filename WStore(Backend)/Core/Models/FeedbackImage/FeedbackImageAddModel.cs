using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Models.ProductVariantImage;

public class FeedbackImageAddModel : IImageAddModel
{
    public IFormFile file { get; set; }
}