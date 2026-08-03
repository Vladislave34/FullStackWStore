using Core.Interfaces;

namespace Core.Models.ProductVariantImage;

public class ProductVariantImageItemModel : IImageItemModel
{
    public Guid Id { get; set; }
    public string Path { get; set; }

    // додатково якщо треба
    public Guid ProductVariantId { get; set; }
}