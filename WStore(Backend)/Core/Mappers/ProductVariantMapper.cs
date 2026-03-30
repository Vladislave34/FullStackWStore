using AutoMapper;
using Core.Models.ProductVariant;
using Domain.Entities;

namespace Core.Mappers;

public class ProductVariantMapper : Profile
{
    public ProductVariantMapper()
    {
        CreateMap<ProductVariantEntity, ProductVariantItemModel>()
            .ForMember(x => x.ProductName,
                opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(x => x.ColorName,
                opt => opt.MapFrom(src => src.Color.Name))
            .ForMember(x => x.SizeName,
                opt => opt.MapFrom(src => src.Size.Name))
            .ForMember(x => x.Images,
                opt => opt.MapFrom(src =>
                    src.Image.Where(i => !i.IsDeleted)
                        .Select(i => i.Path)
                ));
        CreateMap<ProductVariantAddUpdateModel, ProductVariantEntity>()
            .ForMember(x => x.Image, 
                opt => opt.Ignore());
    }
}