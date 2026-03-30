using AutoMapper;
using Core.Models.Size;
using Domain.Entities;

namespace Core.Mappers;

public class SizeMapper : Profile
{
    public SizeMapper()
    {
        CreateMap<SizeEntity, SizeItemModel>();
        CreateMap<SizeAddUpdateModel, SizeEntity>();
    }
}