using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Color;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class ColorService(AppStoreContext context, IMapper mapper) : IColorService
{
    public async Task<IEnumerable<ColorItemModel>> GetAllColors(string lang)
    {
        
          var colors =   await context.Colors
                .Where(d=> d.IsDeleted == false)
                .ProjectTo<ColorItemModel>(mapper.ConfigurationProvider)
                .ToListAsync();
          var localized = colors.Select(x => Localize(x, lang)).ToList();
          return localized;
    }

    public async Task AddColor(ColorAddUpdateModel model)
    {
        var entity = mapper.Map<ColorEntity>(model);
        await context.Colors.AddAsync(entity);
        await context.SaveChangesAsync();
        
    }

    public async Task UpdateColor(ColorAddUpdateModel model)
    {
        var entity = await context.Colors.SingleOrDefaultAsync(x => x.Name == model.Name);
        mapper.Map(model, entity); 
        entity.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        await context.SaveChangesAsync();
    }

    public async Task RemoveColor(Guid id)
    {
        var old_entity = await context.Colors.SingleOrDefaultAsync(x=> x.Id == id);
        old_entity.IsDeleted = true;
        await context.SaveChangesAsync();
    }

    public async Task RemoveAllColors()
    {
        foreach (var entity in await context.Colors.ToListAsync())
        {
            entity.IsDeleted = true;
        }
        await context.SaveChangesAsync();
    }
    public async Task<ColorItemModel> GetColorById(Guid id, string lang)
    {
        var entity = await context.Colors.FirstOrDefaultAsync(x=> x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new Exception("Product not found");
        }
        var color = mapper.Map<ColorItemModel>(entity);
        var localized = Localize(color, lang);
        return localized;
    }
    private static ColorItemModel Localize(ColorItemModel model, string lang)
    {
        if (lang.StartsWith("uk", StringComparison.OrdinalIgnoreCase))
            model.Name = model.NameUk ?? model.Name;

        return model;
    }
}