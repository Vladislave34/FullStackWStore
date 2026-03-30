using AutoMapper;
using Core.Models.ProductVariantImage;
using Domain.Entities;

namespace Core.Mappers;

public class ProductVariantImageMapper : Profile
{
    public ProductVariantImageMapper()
    {
        CreateMap<ProductVariantImageAddModel, ProductVariantImageEntity>();
        CreateMap<ProductVariantImageEntity, ProductVariantImageItemModel>();
    }
}