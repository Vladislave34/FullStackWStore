using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Product.Adrress;
using Core.Models.Product.Payment;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class PaymentService(IAuthService authService, AppStoreContext context, IMapper mapper, IRedisService redisService) : IPaymentService
{
    private const string key1 = "payments";
    
    
    public async Task<ICollection<PaymentItemModel>> GetCardByUser()
    {
        Guid userId = await authService.GetUserIdAsync();
        
        var cache = await redisService.GetAsync<ICollection<PaymentItemModel>>($"{key1}:{userId}");
        if (cache != null) return cache;
        
        
        var items = await context.Payments
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .ProjectTo<PaymentItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        await redisService.SetAsync($"{key1}:{userId}", items,  TimeSpan.FromMinutes(10));
        return items;
    }
    public async Task AddCard(PaymentAddUpdateModel model)
    {
        if(model == null) return;
        Guid userId = await authService.GetUserIdAsync();
        var entity = mapper.Map<PaymentEntity>(model);
        entity.UserId = userId;
        await context.Payments.AddAsync(entity);
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"{key1}:{userId}");
        
    }

    public async Task UpdateCard(Guid id, PaymentAddUpdateModel model)
    {
        if(model == null) return;
        Guid userId = await authService.GetUserIdAsync();
        var entity = await context.Payments.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
        mapper.Map(model, entity);
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"{key1}:{userId}");
    }

    public async Task DeleteCard(Guid id)
    {
        Guid userId = await authService.GetUserIdAsync();
        var entity = await context.Payments.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
        if (entity == null) return;
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"{key1}:{userId}");
    }
}