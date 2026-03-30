using AutoMapper;
using Core.Models.Color;
using Domain.Entities;

namespace Core.Mappers;

public class ColorMapper : Profile
{
    public ColorMapper()
    {
        CreateMap<ColorEntity, ColorItemModel>();
        CreateMap<ColorAddUpdateModel, ColorEntity>();

    }
}