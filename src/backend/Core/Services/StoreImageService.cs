using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Store;
using Core.Models.StoreImage;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class StoreImageService(AppStoreContext context, IMinioImageService imageService, IMapper mapper ) : IStoreImageService
{
    public async Task<List<StoreImageItemModel>> GetAllImages(Guid storeId)
    {
        var images = await context.StoreImages
            .Where(x => x.StoreId == storeId)
            .ProjectTo<StoreImageItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        return images;
    }

    public async Task AddAllImages(Guid storeId, List<StoreImageAddUpdateModel> images)
    {
        var entities = new List<StoreImageEntity>();
        foreach (var image in images)
        {
            var path = await imageService.UploadImageAsync(image.file);
            var entity = new StoreImageEntity
            {
                StoreId = storeId,
                Path = path
            };
            entities.Add(entity);
        }
        
        await context.StoreImages.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAllImages(Guid storeId)
    {
        var entity = await context.Stores
            .Include(s => s.Images)
            .FirstOrDefaultAsync(s => s.Id == storeId);
        foreach (var ent in entity.Images)
        {
            await imageService.DeleteImageAsync(ent.Path);
            ent.IsDeleted = true;
        }
        await context.SaveChangesAsync();
    }
    public async Task DeleteImage(Guid id)
    {
        var entity = await context.StoreImages.FirstOrDefaultAsync(x => x.Id == id);
        
        await imageService.DeleteImageAsync(entity.Path);
        entity.IsDeleted = true;
        
        await context.SaveChangesAsync();
    }

    public async Task UpdateAllImages(Guid storeId, List<StoreImageAddUpdateModel> images)
    {
        
        await DeleteAllImages(storeId);
        await AddAllImages(storeId, images);
    }
}