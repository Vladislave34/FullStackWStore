using AutoMapper;
using Core.Models.Product;
using Domain;
using Domain.Entities;

namespace Core.Mappers;

public class ProductMapper  : Profile 
{
    public ProductMapper()
    {
        CreateMap<ProductEntity, ProductItemModel>()
            .ForMember(x => x.Category,
                opt => opt.MapFrom(src => src.CategoryEntity.Name))
            .ForMember(x=>x.Store,
                opt=>opt.MapFrom(src=>src.Store.Name))
            ;
        CreateMap<ProductAddUpdateModel, ProductEntity>();
    }
}