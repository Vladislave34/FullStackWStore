namespace Core.Interfaces;

public interface IEntityImageService<TEntity, TAddModel, TItemModel>
    where TEntity : class, IImageEntity
    where TAddModel : class, IImageAddModel
    where TItemModel : class, IImageItemModel
{
    Task<List<TItemModel>> GetAllImages(Guid parentId);
    Task AddAllImages(Guid parentId, List<TAddModel> images);
    Task DeleteAllImages(Guid parentId);
    Task DeleteImage(Guid id);
    Task UpdateAllImages(Guid parentId, List<TAddModel> images);
}