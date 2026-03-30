using Core.Models.ProductVariant;

namespace Core.Interfaces;

public interface IProductVariantServices
{
    Task AddProductVariant(ProductVariantAddUpdateModel model);
    Task UpdateProductVariant(Guid id, ProductVariantAddUpdateModel model);
    Task RemoveProductVariant(Guid id);
    Task RemoveAllProductVariants();
    
    Task<IEnumerable<ProductVariantItemModel>> GetAllProductVariantsByProductId(Guid productId);
    
    Task<ProductVariantItemModel> GetProductVariantById(Guid id);
}