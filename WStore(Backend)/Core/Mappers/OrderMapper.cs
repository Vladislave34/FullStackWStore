using AutoMapper;
using Core.Models.Cart;
using Core.Models.Order;
using Core.Models.Product.Payment;
using Domain.Entities;

namespace Core.Mappers;

public class OrderMapper : Profile
{
    public OrderMapper()
    {
        CreateMap<OrderEntity, OrderItemModel>()
            .ForMember(x=>x.OrderStatus,
                opt=>opt.MapFrom(x=>x.Status.ToString()))
            .ForMember(x=>x.Address,
            opt=> opt.MapFrom(src=>
                $"{src.Adrress.Country}, {src.Adrress.City}, {src.Adrress.Street}/{src.Adrress.HouseNumber}"))
            .ForMember(x => x.Items,
                opt => opt.MapFrom(src => src.Items));
        CreateMap<PaymentEntity, PaymentItemModel>();
    }
}