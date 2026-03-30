using AutoMapper;
using Core.Models.Category;
using Domain.Entities;

namespace Core.Mappers;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<CategoryEntity, CategoryItemModel>();
        CreateMap<CategoryAddUpdateModel, CategoryEntity>();
    }
}