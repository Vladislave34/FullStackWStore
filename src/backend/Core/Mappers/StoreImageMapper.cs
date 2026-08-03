using AutoMapper;
using Core.Models.StoreImage;
using Domain.Entities;

namespace Core.Mappers;

public class StoreImageMapper : Profile
{
    public StoreImageMapper()
    {
        CreateMap<StoreImageAddUpdateModel, StoreImageEntity>();
        CreateMap<StoreImageEntity, StoreImageItemModel>();}
}