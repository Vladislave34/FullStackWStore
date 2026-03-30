using AutoMapper;
using Core.Models.Store;
using Domain.Entities;

namespace Core.Mappers;

public class StoreMapper : Profile
{
    public StoreMapper()
    {
        CreateMap<StoreEntity, StoreItemModel>()
            .ForMember(x => x.Images,
                opt => 
                    opt.MapFrom(src => src.Images.Where(i => !i.IsDeleted)
                        .Select(i => i.Path)
                        .ToList()
                        )
                    );

        
        CreateMap<StoreAddUpdateModel, StoreEntity>()
            .ForMember(x => x.Images, opt => opt.Ignore());
    }
}