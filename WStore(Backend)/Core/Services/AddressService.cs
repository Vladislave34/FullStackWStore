using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Product.Adrress;
using Core.Models.Product.Payment;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class AddressService(IAuthService authService, AppStoreContext context, IMapper mapper, IRedisService redisService) : IAddressService
{
    private const string key1 = "adrresses";
    public async Task AddAddress(AddressAddUpdateModel model)
    {
        Guid userId = await authService.GetUserIdAsync();
        var entity = mapper.Map<AdrressEntity>(model);
        entity.UserId = userId;
        await context.Adrresses.AddAsync(entity);
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"{key1}:{userId}");

    }

    public async Task<ICollection<AddressItemModel>> GetAddressByUser()
    {
        Guid userId = await authService.GetUserIdAsync();
        var cache = await redisService.GetAsync<ICollection<AddressItemModel>>($"{key1}:{userId}");
        if (cache != null) return cache;
        
        
        var items = await context.Adrresses
            .Where(x => x.UserId == userId && !x.IsDeleted )
            .ProjectTo<AddressItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        await redisService.SetAsync($"{key1}:{userId}", items,  TimeSpan.FromMinutes(10));
        return items;
    }

    public async Task UpdateAdrress(Guid id, AddressAddUpdateModel model)
    {
        if(model == null) return;
        Guid userId = await authService.GetUserIdAsync();
        var entity = await context.Adrresses.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
        mapper.Map(model, entity);
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"{key1}:{userId}");
    }

    public async Task DeleteAdrress(Guid id)
    {
        Guid userId = await authService.GetUserIdAsync();
        var entity = await context.Adrresses.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
        if (entity == null) return;
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"{key1}:{userId}");
    }
}