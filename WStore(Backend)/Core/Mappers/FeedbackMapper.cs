using AutoMapper;
using Core.Models.Feedback;
using Domain.Entities;

namespace Core.Mappers;

public class FeedbackMapper : Profile
{
    public FeedbackMapper()
    {
        CreateMap<FeedbackEntity, FeedbackItemModel>()
            .ForMember(x => x.Images,
                opt => opt.MapFrom(s => s.Images.Where(i => !i.IsDeleted).Select(f => f.Path)));
           
        CreateMap<FeedbackAddUpdateModel, FeedbackEntity>()
            .ForMember(x => x.Images,
                opt => opt.Ignore());
    }
}