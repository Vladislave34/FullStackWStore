using Core.Models.Product.Adrress;

namespace Core.Interfaces;

public interface IAddressService
{
    Task AddAddress(AddressAddUpdateModel model);
    Task<ICollection<AddressItemModel>> GetAddressByUser();

    Task UpdateAdrress(Guid id, AddressAddUpdateModel model);
    Task DeleteAdrress(Guid id);
}