using AutoMapper;
using TrabajoFinalDyAW.DTOs;
using TrabajoFinalDyAW.Models;
using TrabajoFinalDyAW.Presenters;

namespace TrabajoFinalDyAW.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<Entities.User, Models.User>()
                .ForMember(
                    dest => dest.UserId,
                    opt => opt.MapFrom(u => u.Id)
                )
                .ForMember(
                    dest => dest.UserUsername,
                    opt => opt.MapFrom(u => u.Username)
                ).ForMember(
                    dest => dest.UserPassword,
                    opt => opt.MapFrom(u => u.Password)
                )
                .ForMember(
                    dest => dest.Userpermisssionclaim,
                    opt => opt.MapFrom(u => u.Claims.Select(c => new Userpermisssionclaim { UserId = u.Id, PermissionclaimName = c }))
                );
            CreateMap<Models.User, Entities.User>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(u => u.UserId)
                )
                .ForMember(
                    dest => dest.Username,
                    opt => opt.MapFrom(u => u.UserUsername)
                ).ForMember(
                    dest => dest.Password,
                    opt => opt.MapFrom(u => u.UserPassword)
                )
                .ForMember(
                    dest => dest.Claims,
                    opt => opt.MapFrom(u => u.Userpermisssionclaim.Select(c => c.PermissionclaimName))
                );
            CreateMap<Entities.User, Presenters.UserPresenter>();
            CreateMap<CreateUserDto, Entities.User>();
        }
    }
}
