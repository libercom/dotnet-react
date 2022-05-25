using AutoMapper;
using common.Dtos;
using domain.Models;

namespace core.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserCreationDto, User>();
        }
    }
}
