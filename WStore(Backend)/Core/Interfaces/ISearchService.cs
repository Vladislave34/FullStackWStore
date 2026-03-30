using Core.Models.Product;
using Domain.Entities;

namespace Core.Interfaces;

public interface ISearchService
{
    Task IndexProductAsync(ProductEntity product);
    Task<List<ProductSearchModel>> SearchAsync(string query,
        string? category = null,
        decimal? minPrice = null,
        decimal? maxPrice = null);
    Task<List<string>> AutocompleteAsync(string prefix);
    Task DeleteProductAsync(Guid id);

}