using AutoMapper;
using common.Dtos;
using domain.Models;

namespace core.Profiles
{
    public class CargoTypeProfile : Profile
    {
        public CargoTypeProfile()
        {
            CreateMap<CargoType, CargoTypeDto>();
            CreateMap<CargoTypeDto, CargoType>();
        }
    }
}
