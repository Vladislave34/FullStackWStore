using Core.Models.Size;
using Core.Models.Store;
using Core.Models.User;

namespace Core.Interfaces;

public interface IStoreService
{
    Task<AuthResponseModel> AddStore(StoreAddUpdateModel model);
    Task UpdateStore(Guid id, StoreAddUpdateModel model);
    Task RemoveStore(Guid id);
    Task RemoveAllStores();
    
    Task<IEnumerable<StoreItemModel>> GetAllStores();
    
    Task<StoreItemModel> GetStoreById(Guid id);
    Task<StoreItemModel> GetStoreByUserId();
}