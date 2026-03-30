using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.StoreImage;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class EntityImageService<TEntity, TAddModel, TItemModel>(
    AppStoreContext context,
    IMinioImageService imageService,
    IMapper mapper) : IEntityImageService<TEntity, TAddModel, TItemModel>
    where TEntity : class, IImageEntity
    where TAddModel : class, IImageAddModel
    where TItemModel : class, IImageItemModel
{
    public async Task<List<TItemModel>> GetAllImages(Guid parentId)
    {
        return await context.Set<TEntity>()
            .Where(x => x.ParentId == parentId && !x.IsDeleted)
            .ProjectTo<TItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task AddAllImages(Guid parentId, List<TAddModel> images)
    {
        var entities = new List<TEntity>();

        foreach (var image in images)
        {
            var path = await imageService.UploadImageAsync(image.file);
            var entity = mapper.Map<TEntity>(image);

            entity.ParentId = parentId;
            entity.Path = path;

            entities.Add(entity);
        }

        await context.Set<TEntity>().AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAllImages(Guid parentId)
    {
        var entities = await context.Set<TEntity>()
            .Where(x => x.ParentId == parentId && !x.IsDeleted)
            .ToListAsync();

        foreach (var ent in entities)
        {
            await imageService.DeleteImageAsync(ent.Path);
            ent.IsDeleted = true;
        }

        await context.SaveChangesAsync();
    }

    public async Task DeleteImage(Guid id)
    {
        var entity = await context.Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
            return;

        await imageService.DeleteImageAsync(entity.Path);
        entity.IsDeleted = true;

        await context.SaveChangesAsync();
    }

    public async Task UpdateAllImages(Guid parentId, List<TAddModel> images)
    {
        await DeleteAllImages(parentId);
        await AddAllImages(parentId, images);
    }
}