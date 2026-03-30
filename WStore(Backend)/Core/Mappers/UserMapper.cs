using AutoMapper;
using Core.Models.User;
using Domain.Entities.Identity;

namespace Core.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        

        CreateMap<UserEntity, UserItemModel>()
            .ForMember(
                x => x.FullName,
                opt =>
                    opt.MapFrom(src => src.FirstName + " " + src.LastName))
            .ForMember(x => x.Roles,
                opt =>
                    opt.MapFrom(src => src.UserRoles!.Select(ur => ur.Role.Name)));

        CreateMap<GoogleUserModel, UserEntity>()
            .ForMember(x => x.Image, opt => opt.Ignore())
            .ForMember(x => x.UserName, opt => opt.MapFrom(x=>x.UserName));


    }
}