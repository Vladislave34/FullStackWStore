using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Product.Gender;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class GenderService(AppStoreContext context, IMapper mapper, IRedisService redisService) : IGenderService
{
    public async Task<List<GenderItemModel>> GetAllGenders(string lng)
    {
        string key = $"genders:all:{lng}";
        var cache = await redisService.GetAsync<List<GenderItemModel>>(key);
        if (cache != null) return cache;

        var genders = await context.Genders
            .Where(x => !x.IsDeleted)
            .ProjectTo<GenderItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        var localized = genders.Select(c => Localize(c, lng)).ToList();
        await redisService.SetAsync(key, localized, TimeSpan.FromMinutes(10));
        return localized;


    }
    private static GenderItemModel Localize(GenderItemModel model, string lang)
    {
        if (lang.StartsWith("uk", StringComparison.OrdinalIgnoreCase))
            model.Name = model.NameUk ?? model.Name;

        return model;
    }
}