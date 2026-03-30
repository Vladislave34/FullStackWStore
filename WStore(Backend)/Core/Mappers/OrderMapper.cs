using AutoMapper;
using Core.Models.Cart;
using Core.Models.Order;
using Domain.Entities;

namespace Core.Mappers;

public class OrderMapper : Profile
{
    public OrderMapper()
    {
        CreateMap<OrderEntity, OrderItemModel>()
            .ForMember(x => x.OrderStatus,
                opt => opt.MapFrom(src => src.OrderStatus.Name))
            .ForMember(x => x.Items,
                opt => opt.MapFrom(src => src.Items));
    }
}