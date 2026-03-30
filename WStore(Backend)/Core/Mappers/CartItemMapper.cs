using AutoMapper;
using Core.Models.CartItem;
using Domain.Entities;

namespace Core.Mappers;

public class CartItemMapper  : Profile
{
    public CartItemMapper()
    {
        CreateMap<CartItemEntity, CartItemItemModel>();
        CreateMap<CartItemAddUpdateModel, CartItemEntity>();
    }
}