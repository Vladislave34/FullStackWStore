using Core.Models.CartItem;
using Core.Models.Product;

namespace Core.Interfaces;

public interface ICartItemService
{
    Task AddCartItem(CartItemAddUpdateModel model);
    Task UpdateCartItem(Guid id, CartItemAddUpdateModel model);
    Task RemoveCartItem(Guid id);
    Task RemoveAllCartItems();
    
    Task<IEnumerable<CartItemItemModel>> GetAllCartItems();
    
    Task<CartItemItemModel> GetCartItemById(Guid id);
    
    Task<IEnumerable<CartItemItemModel>> GetCartItemsByUser();
    Task<PageResult<CartItemItemModel>> SearchCartItems(
        string query, string lang, int pageNumber = 1, int pageSize = 10);
}