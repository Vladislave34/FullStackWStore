using AutoMapper;
using Core.Models.ProductVariantImage;
using Domain.Entities;

namespace Core.Mappers;

public class FeedbackImageMapper : Profile
{
    public FeedbackImageMapper()
    {
        CreateMap<FeedbackImageAddModel, FeedbackImageEntity>();
        CreateMap<FeedbackImageEntity, FeedbackImageItemModel>();
    }
}