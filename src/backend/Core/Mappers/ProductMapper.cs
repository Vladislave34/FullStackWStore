using AutoMapper;
using Core.Models.Product;
using Core.Models.ProductVariant;
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
            .ForMember(x => x.CategoryUk,
                opt => opt.MapFrom(src => src.CategoryEntity.NameUk))
            .ForMember(x => x.Store,
                opt => opt.MapFrom(src => src.Store.Name))
            .ForMember(x=>x.Gender,
                opt=>opt.MapFrom(src=>src.GenderEntity.Name))
            .ForMember(x=>x.GenderUk,
                opt=>opt.MapFrom(src=>src.GenderEntity.NameUk))
            ;
        CreateMap<ProductVariantEntity, ProductVariantItemModel>()
            .ForMember(x=>x.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(x=>x.ProductNameUk, opt => opt.MapFrom(src => src.Product.NameUk))
            .ForMember(x => x.ColorName, opt => opt.MapFrom(src => src.Color.Name))
            .ForMember(x => x.ColorNameUk, opt => opt.MapFrom(src => src.Color.NameUk))
            .ForMember(x => x.SizeName, opt => opt.MapFrom(src => src.Size.Name))
            .ForMember(dest => dest.Sale,
                opt => opt.MapFrom(src => src.Sale.Percent == null ? 0 : src.Sale.Percent));
        CreateMap<ProductAddUpdateModel, ProductEntity>();
        CreateMap<ProductEntity, ProductSearchModel>();
        CreateMap<ProductVariantEntity, ProductVariantSearchModel>();
    }
}