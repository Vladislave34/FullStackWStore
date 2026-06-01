using Core.Models.Product;

namespace Core.Interfaces;

public interface IProductService
{
    Task AddProduct(ProductAddUpdateModel model);
    Task UpdateProduct(Guid id, ProductAddUpdateModel model);
    Task RemoveProduct(Guid id);
    Task RemoveAllProducts();
    
    Task<IEnumerable<ProductItemModel>> GetAllProducts(string lang);
    
    Task<ProductItemModel> GetProductById(Guid id, string lang);
}