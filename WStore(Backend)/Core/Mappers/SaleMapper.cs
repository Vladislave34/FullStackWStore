using AutoMapper;
using Core.Models.Product.Sale;
using Domain.Entities;

namespace Core.Mappers;

public class SaleMapper : Profile
{
    public SaleMapper()
    {
        CreateMap<SaleEntity, SaleItemModel>();
    }
}