using AutoMapper;
using Bogus.DataSets;
using Core.Models.Product.Gender;
using Domain.Entities;

namespace Core.Mappers;

public class GenderMapper : Profile
{
    public GenderMapper()
    {
        CreateMap<GenderEntity, GenderItemModel>();
    }
}