using Core.Models.Product;

namespace Core.Interfaces;

public interface IFavoutiteService
{
    Task AddFavourite(Guid favouriteId);
    Task RemoveFavourite( Guid productId);
    Task<bool> IsFavourite( Guid productId);
    Task<List<ProductItemModel>> GetFavourites(string lang);
}