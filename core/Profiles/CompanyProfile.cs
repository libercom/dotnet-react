using AutoMapper;
using common.Dtos;
using domain.Models;

namespace core.Profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>();
            CreateMap<CompanyDto, Company>();
        }
    }
}
