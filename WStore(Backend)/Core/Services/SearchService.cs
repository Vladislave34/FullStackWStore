using AutoMapper;
using Core.Interfaces;
using Core.Models.CartItem;
using Core.Models.Product;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace Core.Services;

public class SearchService(IElasticClient elasticClient, IMapper mapper, AppStoreContext context) : ISearchService
{
    private const string Index = "products";
    private const string IndexCartItem = "cart-items";

    public async Task IndexProductAsync(ProductEntity product)
    {
        var doc = mapper.Map<ProductSearchModel>(product);
        
        var response = await elasticClient.IndexAsync(doc,  idx => 
            idx.Index(Index).Id(product.Id)
            );
        if (!response.IsValid)
        {
            
            throw new Exception($"Не вдалося проіндексувати товар {product.Id}");
        }
    }
    public async Task ReindexAllAsync()
    {
        var products = await context.Products
            .Include(p => p.Variants)
            .ToListAsync();

        foreach (var product in products)
        {
            await IndexProductAsync(product);
        }
    }
    public async Task EnsureIndexCreatedAsync()
    {
        var existsResponse = await elasticClient.Indices.ExistsAsync(Index);
        if (existsResponse.Exists)
            return;

        var createResponse = await elasticClient.Indices.CreateAsync(Index, c => c
            .Map<ProductSearchModel>(m => m
                .Properties(p => p
                    .Keyword(k => k.Name(n => n.CategoryId))
                    .Keyword(k => k.Name(n => n.GenderId))
                    .Keyword(k => k.Name(n => n.StoreId))
                    .Nested<ProductVariantSearchModel>(n => n
                        .Name(x => x.Variants)
                        .Properties(pp => pp
                            .Keyword(k => k.Name(v => v.ColorId))
                            .Keyword(k => k.Name(v => v.SizeId))
                        )
                    )
                )
            )
        );

        if (!createResponse.IsValid)
            throw new Exception($"Не вдалося створити індекс: {createResponse.DebugInformation}");
    }

    public async Task IndexCartItemAsync(CartItemEntity cartItem)
    {
        var entity = await context.CartItems
            .Include(x=>x.ProductVariant)
            .Include(x=>x.ProductVariant.Color)
            .Include(x=>x.ProductVariant.Size)
            .Include(x=>x.ProductVariant.Sale)
            .Include(x=>x.ProductVariant.Image)
            .Include(x=>x.ProductVariant.Product)
            .FirstOrDefaultAsync(x=> x.Id == cartItem.Id && !x.IsDeleted);
        var doc = mapper.Map<CartItemItemModel>(entity);
        var response = await elasticClient.IndexAsync(doc, idx =>
            idx.Index(IndexCartItem).Id(cartItem.Id)
        );
        if (!response.IsValid)
        {
            
            throw new Exception($"Не вдалося проіндексувати товар {cartItem.Id}");
        }

    }

    public async Task<(List<Guid> Ids, int TotalCount)> SearchAsync(
        string query, string lang, Guid? storeId, Guid? categoryId, int pageNumber, int pageSize)
    {
        Console.WriteLine($"[DEBUG ENTER] query={query}, lang={lang}, storeId={storeId}, categoryId={categoryId}");
    
       
    
        
        var nameField = lang.StartsWith("uk") ? "nameUk" : "name";
        var descField = lang.StartsWith("uk") ? "descriptionUk" : "description";

        var filters = new List<Func<QueryContainerDescriptor<ProductSearchModel>, QueryContainer>>();

        if (storeId.HasValue)
            filters.Add(f => f.Term(t => t
                .Field("storeId.keyword")
                .Value(storeId.Value.ToString())));

        if (categoryId.HasValue)
            filters.Add(f => f.Term(t => t
                .Field("categoryId.keyword")
                .Value(categoryId.Value.ToString())));

        var response = await elasticClient.SearchAsync<ProductSearchModel>(s => s
            .Index(Index)
            .From((pageNumber - 1) * pageSize)
            .Size(pageSize)
            .Query(q => q.Bool(b => b
                .Must(m => m.MultiMatch(mm => mm
                    .Query(query)
                    .Fields(new[] { $"{nameField}^2", descField })
                    .Fuzziness(Fuzziness.Auto)))
                .Filter(filters.ToArray())
            ))
        );
        Console.WriteLine($"[DEBUG QUERY] {response.ApiCall.HttpMethod} {response.ApiCall.Uri}");
        Console.WriteLine($"[DEBUG BODY] {(response.ApiCall.RequestBodyInBytes != null ? System.Text.Encoding.UTF8.GetString(response.ApiCall.RequestBodyInBytes) : "EMPTY")}");


        if (!response.IsValid)
            throw new Exception($"Помилка пошуку: {response.DebugInformation}");

        var ids = response.Documents.Select(d => d.Id).ToList();
        Console.WriteLine($"[DEBUG RESULT] ids.Count={ids.Count}, total={response.Total}");
        return (ids, (int)response.Total);
    }
    /*
    public async Task<(List<Guid> Ids, int TotalCount)> SearchAsync(
        string? query, string lang, Guid? categoryId, Guid? genderId, int a, int pageNumber, int pageSize)
    {
        var nameField = lang.StartsWith("uk") ? "nameUk" : "name";
        var descField = lang.StartsWith("uk") ? "descriptionUk" : "description";

        var filters = new List<Func<QueryContainerDescriptor<ProductSearchModel>, QueryContainer>>();

        if (categoryId.HasValue)
            filters.Add(f => f.Term(t => t
                .Field("categoryId.keyword")
                .Value(categoryId.Value.ToString())));

        if (genderId.HasValue)
            filters.Add(f => f.Term(t => t
                .Field("genderId.keyword")
                .Value(genderId.Value.ToString())));

        

        var response = await elasticClient.SearchAsync<ProductSearchModel>(s => s
            .Index(Index)
            .From((pageNumber - 1) * pageSize)
            .Size(pageSize)
            .Query(q => q.Bool(b =>
            {
                b.Filter(filters.ToArray());

                if (!string.IsNullOrWhiteSpace(query))
                {
                    b.Must(m => m.MultiMatch(mm => mm
                        .Query(query)
                        .Fields(new[] { $"{nameField}^2", descField })
                        .Fuzziness(Fuzziness.Auto)));
                }
                else
                {
                    b.Must(m => m.MatchAll());
                }

                return b;
            }))
        );

        if (!response.IsValid)
            throw new Exception($"Помилка пошуку: {response.DebugInformation}");

        var ids = response.Documents.Select(d => d.Id).ToList();
        return (ids, (int)response.Total);
    }*/
    
    public async Task<(List<Guid> Ids, int TotalCount)> SearchAsync(
    string? query, string lang, Guid? categoryId, Guid? genderId,
    Guid? colorId, Guid? sizeId,
    int pageNumber, int pageSize)
{
    var nameField = lang.StartsWith("uk") ? "nameUk" : "name";
    var descField = lang.StartsWith("uk") ? "descriptionUk" : "description";

    var filters = new List<Func<QueryContainerDescriptor<ProductSearchModel>, QueryContainer>>();

    if (categoryId.HasValue)
        filters.Add(f => f.Term(t => t
            .Field(p => p.CategoryId)
            .Value(categoryId.Value)));

    if (genderId.HasValue)
        filters.Add(f => f.Term(t => t
            .Field(p => p.GenderId)
            .Value(genderId.Value)));

    if (colorId.HasValue || sizeId.HasValue)
    {
        filters.Add(f => f.Nested(n => n
            .Path(p => p.Variants)
            .Query(nq => nq.Bool(nb =>
            {
                var variantFilters = new List<Func<QueryContainerDescriptor<ProductSearchModel>, QueryContainer>>();

                if (colorId.HasValue)
                    variantFilters.Add(vf => vf.Term(t => t
                        .Field("variants.colorId")
                        .Value(colorId.Value)));

                if (sizeId.HasValue)
                    variantFilters.Add(vf => vf.Term(t => t
                        .Field("variants.sizeId")
                        .Value(sizeId.Value)));

                nb.Filter(variantFilters.ToArray());
                return nb;
            }))
        ));
    }

    var response = await elasticClient.SearchAsync<ProductSearchModel>(s => s
        .Index(Index)
        .From((pageNumber - 1) * pageSize)
        .Size(pageSize)
        .Query(q => q.Bool(b =>
        {
            b.Filter(filters.ToArray());

            if (!string.IsNullOrWhiteSpace(query))
            {
                b.Must(m => m.MultiMatch(mm => mm
                    .Query(query)
                    .Fields(new[] { $"{nameField}^2", descField })
                    .Fuzziness(Fuzziness.Auto)));
            }
            else
            {
                b.Must(m => m.MatchAll());
            }

            return b;
        }))
    );

    if (!response.IsValid)
        throw new Exception($"Помилка пошуку: {response.DebugInformation}");

    var ids = response.Documents.Select(d => d.Id).ToList();
    return (ids, (int)response.Total);
}

    public async Task<(List<Guid> Ids, int TotalCount)> SearchCartItemAsync(
        string query, string lang, int pageNumber, int pageSize)
    {
        var nameField = lang.StartsWith("uk") ? "nameUk" : "name";

        var response = await elasticClient.SearchAsync<CartItemItemModel>(s => s
            .Index(IndexCartItem)
            .From((pageNumber - 1) * pageSize)
            .Size(pageSize)
            .Query(q => q.Match(mm => mm
                .Field(nameField)
                .Query(query)
                .Fuzziness(Fuzziness.Auto)))
        );

        if (!response.IsValid)
            throw new Exception($"Помилка пошуку: {response.DebugInformation}");

        var ids = response.Documents.Select(d => d.Id).ToList();
        return (ids, (int)response.Total);
    }
    
    
    public async Task DeleteProductAsync(Guid id)
    {
        await elasticClient.DeleteAsync<ProductEntity>(id, idx=>idx.Index(Index));
    }
}