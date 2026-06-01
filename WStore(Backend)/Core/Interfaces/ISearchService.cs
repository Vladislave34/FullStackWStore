using Core.Models.Product;
using Domain.Entities;

namespace Core.Interfaces;

public interface ISearchService
{
    Task IndexProductAsync(ProductEntity product);
    Task<List<ProductSearchModel>> SearchAsync(
        string query,
        string lang = "en",
        string? category = null,
        decimal? minPrice = null,
        decimal? maxPrice = null);
    Task<List<string>> AutocompleteAsync(string prefix, string lang = "en");
    Task DeleteProductAsync(Guid id);
}