using Core.Models.CartItem;

namespace Core.Interfaces;

public interface ICartItemService
{
    Task AddCartItem(CartItemAddUpdateModel model);
    Task UpdateCartItem(Guid id, CartItemAddUpdateModel model);
    Task RemoveCartItem(Guid id);
    Task RemoveAllCartItems();
    
    Task<IEnumerable<CartItemItemModel>> GetAllCartItems();
    
    Task<CartItemItemModel> GetCartItemById(Guid id);
}