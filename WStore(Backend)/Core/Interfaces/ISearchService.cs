using Core.Models.Product;
using Domain.Entities;

namespace Core.Interfaces;

public interface ISearchService
{
    Task IndexProductAsync(ProductEntity product);
    Task IndexCartItemAsync(CartItemEntity cartItem);

    Task EnsureIndexCreatedAsync();
    
    Task DeleteProductAsync(Guid id);
    Task ReindexAllAsync();
    
    Task<(List<Guid> Ids, int TotalCount)> SearchAsync(
        string query, string lang, Guid? storeId, Guid? categoryId, int pageNumber, int pageSize);

    Task<(List<Guid> Ids, int TotalCount)> SearchAsync(
        string? query, string lang, Guid? categoryId, Guid? genderId,
        Guid? colorId, Guid? sizeId,
        int pageNumber, int pageSize);

    Task<(List<Guid> Ids, int TotalCount)> SearchCartItemAsync(
        string query, string lang, int pageNumber, int pageSize);

}