using Core.Models.Cart;

namespace Core.Interfaces;

public interface ICartService
{
    Task AddCart();
    Task UpdateCart(Guid id, CartAddUpdateModel model);
    Task RemoveCart(Guid id);
    Task RemoveAllCarts();
    
    Task<IEnumerable<CartItemModel>> GetAllCarts();
    
    Task<CartItemModel> GetCartById(Guid id);
}