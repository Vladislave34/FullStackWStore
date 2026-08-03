using Microsoft.AspNetCore.Http;

namespace Core.Models.ProductVariant;

public sealed record ProductVariantAddUpdateModel
{
    
    public Guid ProductId { get; set; }
    

    public Guid ColorId { get; set; }
    

    public Guid SizeId { get; set; }
    public Guid? SaleId { get; set; }
    
    
    public decimal Price { get; set; }
    public List<IFormFile> Images { get; set; }
}