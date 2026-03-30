using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Models.ProductVariantImage;

public class ProductVariantImageAddModel : IImageAddModel
{
    public IFormFile file { get; set; }
}