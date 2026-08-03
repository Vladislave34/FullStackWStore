using Core.Models.Product;
using Domain.Entities;

namespace Core.Interfaces;

public interface IProductService
{
    Task AddProduct(ProductAddUpdateModel model);
    Task UpdateProduct(Guid id, ProductAddUpdateModel model);
    Task RemoveProduct(Guid id);
    Task RemoveAllProducts();
    
    Task<PageResult<ProductItemModel>> GetAllProducts(string lang,  int pageNumber = 1, int pageSize = 10);

    Task<PageResult<ProductItemModel>> GetAllProducts(string lang, string? query,
        Guid? categoryId, Guid? genderId, Guid? colorId, Guid? sizeId,
        int pageNumber = 1, int pageSize = 10);    
    Task<ProductItemModel> GetProductById(Guid id, string lang);
    
    Task<PageResult<ProductItemModel>> GetProductsByStoreId(Guid storeId,  string lang,   int pageNumber = 1, int pageSize = 10);

    Task<PageResult<ProductItemModel>> GetProductsByStoreId(Guid storeId, string lang, Guid categoryId,
        int pageNumber = 1, int pageSize = 10);

    Task<PageResult<ProductItemModel>> GetAllProducts(string lang, string? query, Guid? categoryId,
        Guid? genderId, bool? hasSale, Guid? colorId, Guid? sizeId,
        int pageNumber = 1, int pageSize = 10);

    Task<PageResult<ProductItemModel>> GetProductsByStoreId(Guid storeId, string lang, Guid? categoryId,
        string searchQuery,
        int pageNumber = 1, int pageSize = 10);
}