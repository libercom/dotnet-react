using AutoMapper;
using core.Dtos;
using domain.Models;

namespace core.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserLoginResponseDto>();
            CreateMap<UserCreationDto, User>();
        }
    }
}
