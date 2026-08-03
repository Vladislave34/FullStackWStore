using AutoMapper;
using Core.Models.Cart;
using Domain.Entities;

namespace Core.Mappers;

public class CartMapper : Profile
{
    public CartMapper()
    {
        CreateMap<CartEntity, CartItemModel>()
            .ForMember(x => x.CartItems,
                opt => opt.MapFrom(src => src.Items.Select(f => f.Id).ToList()));
        CreateMap<CartAddUpdateModel, CartEntity>();
    }
}