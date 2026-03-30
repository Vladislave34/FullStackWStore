using Core.Models.Category;

namespace Core.Interfaces;

public interface ICategoryService
{
    Task AddCategory(CategoryAddUpdateModel model);
    Task UpdateCategory(Guid id, CategoryAddUpdateModel model);
    Task RemoveCategory(Guid id);
    Task RemoveAllCategories();
    
    Task<IEnumerable<CategoryItemModel>> GetAllCategories();
    
    Task<CategoryItemModel> GetCategoryById(Guid id);
}