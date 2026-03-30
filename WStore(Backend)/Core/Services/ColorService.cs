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
    public async Task<IEnumerable<ColorItemModel>> GetAllColors()
    {
        return
            await context.Colors
                .Where(d=> d.IsDeleted == false)
                .ProjectTo<ColorItemModel>(mapper.ConfigurationProvider)
                .ToListAsync();
        
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
    public async Task<ColorItemModel> GetColorById(Guid id)
    {
        var entity = await context.Colors.FirstOrDefaultAsync(x=> x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new Exception("Product not found");
        }
        return mapper.Map<ColorItemModel>(entity);
    }
}