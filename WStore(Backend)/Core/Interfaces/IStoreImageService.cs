using Core.Models.StoreImage;

namespace Core.Interfaces;

public interface IStoreImageService
{
    Task<List<StoreImageItemModel>> GetAllImages(Guid storeId);
    Task AddAllImages(Guid storeId, List<StoreImageAddUpdateModel> images);
    Task DeleteAllImages(Guid storeId);
    Task UpdateAllImages(Guid storeId, List<StoreImageAddUpdateModel> images);
    Task DeleteImage(Guid id);
}