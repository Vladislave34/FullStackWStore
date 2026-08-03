using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Product.Sale;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class SaleService(AppStoreContext context, IMapper mapper, IRedisService redisService) : ISaleService
{
    public async Task<List<SaleItemModel>> GetSales()
    {
        string key = $"sales:all";
        var cache = await redisService.GetAsync<List<SaleItemModel>>(key);
        if (cache != null) return cache;

        var sales = await context.Sales
            .Where(x => !x.IsDeleted)
            .ProjectTo<SaleItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        await redisService.SetAsync(key, sales,  TimeSpan.FromMinutes(10));
        return sales;
    }
    
}