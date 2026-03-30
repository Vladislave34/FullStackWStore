using Core.Interfaces;
using Core.Models.Product;
using Domain.Entities;
using Nest;

namespace Core.Services;

public class SearchService(IElasticClient elasticClient) : ISearchService
{
    private const string Index = "products";

    
    public async Task IndexProductAsync(ProductEntity product)
    {
        var model = new ProductSearchModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            CategoryName = product.CategoryEntity?.Name,
            StoreName = product.Store?.Name,
            MinPrice = product.Variants.Any()
                ? product.Variants.Min(v => v.Price)
                : 0,
            Colors = product.Variants
                .Select(v => v.Color?.Name)
                .Where(x => x != null)
                .Distinct()
                .ToList(),
            Sizes = product.Variants
                .Select(v => v.Size?.Name)
                .Where(x => x != null)
                .Distinct()
                .ToList()
        };

        await elasticClient.IndexAsync(model, i => i.Index(Index));
    }

    // пошук
    public async Task<List<ProductSearchModel>> SearchAsync(string query, 
        string? category = null,
        decimal? minPrice = null, 
        decimal? maxPrice = null)
    {
        var result = await elasticClient.SearchAsync<ProductSearchModel>(s => s
                .Index(Index)
                .Query(q => q
                    .Bool(b =>
                    {
                       
                        if (!string.IsNullOrEmpty(query))
                        {
                            b.Must(m => m
                                .MultiMatch(mm => mm
                                        .Fields(f => f
                                            .Field(p => p.Name, boost: 3)
                                            .Field(p => p.Description)
                                            .Field(p => p.CategoryName)
                                        )
                                        .Query(query)
                                        .Fuzziness(Fuzziness.EditDistance(1)) // ← замість Auto
                                )
                            );
                        }
                        else
                        {
                            b.Must(m => m.MatchAll()); // ← повертає всі
                        }

                        if (!string.IsNullOrEmpty(category))
                            b.Filter(f => f
                                .Term(t => t.Field(p => p.CategoryName.Suffix("keyword"))
                                    .Value(category)));

                        if (minPrice.HasValue || maxPrice.HasValue)
                            b.Filter(f => f
                                .Range(r => r
                                    .Field(p => p.MinPrice)
                                    .GreaterThanOrEquals((double?)minPrice)
                                    .LessThanOrEquals((double?)maxPrice)));

                        return b;
                    })
                )
                .Sort(s => s.Descending(SortSpecialField.Score))
                .Size(100) 
        );

        return result.Documents.ToList();
    }

    
    public async Task<List<string>> AutocompleteAsync(string prefix)
    {
        var result = await elasticClient.SearchAsync<ProductSearchModel>(s => s
            .Index(Index)
            .Query(q => q
                .MatchPhrasePrefix(m => m
                    .Field(f => f.Name)
                    .Query(prefix)
                )
            )
            .Size(5)
        );

        return result.Documents.Select(x => x.Name).ToList();
    }

    
    public async Task DeleteProductAsync(Guid id)
    {
        await elasticClient.DeleteAsync<ProductSearchModel>(id, d => d.Index(Index));
    }
}