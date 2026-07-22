using AutoMapper;
using Core.Models.Cart;
using Core.Models.CartItem;
using Core.Models.OrderItem;
using Domain.Entities;

namespace Core.Mappers;

public class OrderItemMapper  :Profile
{
    public OrderItemMapper()
    {
        CreateMap<OrderItemEntity, OrderItemItemModel>()
            .ForMember(x => x.ProductName,
                opt => opt.MapFrom(src => src.ProductVariant.Product.Name))
            .ForMember(x => x.ProductNameUk,
                opt => opt.MapFrom(src => src.ProductVariant.Product.NameUk))
            .ForMember(x=>x.ColorNameUk,
                opt => opt.MapFrom(src => src.ProductVariant.Color.NameUk))
            .ForMember(x => x.ColorName,
                opt => opt.MapFrom(src => src.ProductVariant.Color.Name))
           .ForMember(x => x.SizeName,
                opt => opt.MapFrom(src => src.ProductVariant.Size.Name))
            .ForMember(x=>x.Images,
            opt=>opt.MapFrom(src=>
                src.ProductVariant.Image.Select(i=>i.Path)))
            ;
    }
}