using Core.Models.Size;

namespace Core.Interfaces;

public interface ISizeService 
{
    Task AddSize(SizeAddUpdateModel model);
    Task UpdateSize(Guid id, SizeAddUpdateModel model);
    Task RemoveSize(Guid id);
    Task RemoveAllSizes();
    
    Task<IEnumerable<SizeItemModel>> GetAllSizes();
    
    Task<SizeItemModel> GetSizeById(Guid id);
}