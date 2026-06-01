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
            NameUk = product.NameUk,
            Description = product.Description,
            DescriptionUk = product.DescriptionUk,
            CategoryName = product.CategoryEntity?.Name,
            CategoryNameUk = product.CategoryEntity?.NameUk,
            StoreName = product.Store?.Name,
            MinPrice = product.Variants.Any()
                ? product.Variants.Min(v => v.Price)
                : 0,
            Colors = product.Variants
                .Select(v => v.Color?.Name)
                .Where(x => x != null)
                .Distinct()
                .ToList()!,
            Sizes = product.Variants
                .Select(v => v.Size?.Name)
                .Where(x => x != null)
                .Distinct()
                .ToList()!
        };

        await elasticClient.IndexAsync(model, i => i.Index(Index));
    }

    public async Task<List<ProductSearchModel>> SearchAsync(
        string query,
        string lang = "en",
        string? category = null,
        decimal? minPrice = null,
        decimal? maxPrice = null)
    {
        bool isUk = lang.StartsWith("uk", StringComparison.OrdinalIgnoreCase);

        var result = await elasticClient.SearchAsync<ProductSearchModel>(s => s
            .Index(Index)
            .Query(q => q
                .Bool(b =>
                {
                    if (!string.IsNullOrEmpty(query))
                    {
                        b.Must(m => m
                            .MultiMatch(mm =>
                            {
                                if (isUk)
                                    mm.Fields(f => f
                                        .Field(p => p.NameUk, boost: 3)
                                        .Field(p => p.DescriptionUk)
                                        .Field(p => p.CategoryNameUk));
                                else
                                    mm.Fields(f => f
                                        .Field(p => p.Name, boost: 3)
                                        .Field(p => p.Description)
                                        .Field(p => p.CategoryName));

                                return mm.Query(query).Fuzziness(Fuzziness.EditDistance(1));
                            })
                        );
                    }
                    else
                    {
                        b.Must(m => m.MatchAll());
                    }

                    if (!string.IsNullOrEmpty(category))
                    {
                        if (isUk)
                            b.Filter(f => f
                                .Term(t => t
                                    .Field(p => p.CategoryNameUk.Suffix("keyword"))
                                    .Value(category)));
                        else
                            b.Filter(f => f
                                .Term(t => t
                                    .Field(p => p.CategoryName.Suffix("keyword"))
                                    .Value(category)));
                    }

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

        var documents = result.Documents.ToList();

        if (isUk)
        {
            documents.ForEach(d =>
            {
                d.Name = d.NameUk ?? d.Name;
                d.Description = d.DescriptionUk ?? d.Description;
                d.CategoryName = d.CategoryNameUk ?? d.CategoryName;
            });
        }

        return documents;
    }

    public async Task<List<string>> AutocompleteAsync(string prefix, string lang = "en")
    {
        bool isUk = lang.StartsWith("uk", StringComparison.OrdinalIgnoreCase);

        var result = await elasticClient.SearchAsync<ProductSearchModel>(s => s
            .Index(Index)
            .Query(q => q
                .MatchPhrasePrefix(m =>
                {
                    if (isUk)
                        m.Field(f => f.NameUk).Query(prefix);
                    else
                        m.Field(f => f.Name).Query(prefix);

                    return m;
                })
            )
            .Size(5)
        );

        return isUk
            ? result.Documents.Select(x => x.NameUk ?? x.Name).ToList()
            : result.Documents.Select(x => x.Name).ToList();
    }

    public async Task DeleteProductAsync(Guid id)
    {
        await elasticClient.DeleteAsync<ProductSearchModel>(id, d => d.Index(Index));
    }
}