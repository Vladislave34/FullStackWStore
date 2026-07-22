
using Core.Models.Product.Adrress;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Core.Mappers;

public class AddressMapper : Profile
{
    public AddressMapper()
    {
        CreateMap<AdrressEntity, AddressItemModel>();
        CreateMap<AddressAddUpdateModel, AdrressEntity>();
    }
}