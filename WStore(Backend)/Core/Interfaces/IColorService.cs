using Core.Models.Color;

namespace Core.Interfaces;

public interface IColorService
{
    Task AddColor(ColorAddUpdateModel model);
    Task UpdateColor(ColorAddUpdateModel model);
    Task RemoveColor(Guid id);
    Task RemoveAllColors();
    
    Task<IEnumerable<ColorItemModel>> GetAllColors();
    
    Task<ColorItemModel> GetColorById(Guid id);
}