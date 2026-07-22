using AutoMapper;
using Core.Models.CartItem;
using Domain.Entities;

namespace Core.Mappers;

public class CartItemMapper  : Profile
{
    public CartItemMapper()
    {
        CreateMap<CartItemEntity, CartItemItemModel>()
            .ForMember(dest => dest.ProductVariant, opt => opt.MapFrom(src => src.ProductVariant))
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.ProductVariant.Product));

        CreateMap<ProductEntity, ProductDto>();

        CreateMap<ProductVariantEntity, ProductVariantDto>()
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color.Name))
            .ForMember(dest => dest.ColorUk, opt => opt.MapFrom(src => src.Color.NameUk))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size.Name))
            .ForMember(dest => dest.Sale, opt => opt.MapFrom(src => src.Sale != null ? src.Sale.Percent : 0))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Image.Select(i => i.Path)));

        CreateMap<CartItemAddUpdateModel, CartItemEntity>();
    }
}