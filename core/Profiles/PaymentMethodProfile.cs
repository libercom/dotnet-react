using AutoMapper;
using core.Dtos;
using domain.Models;

namespace core.Profiles
{
    public class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            CreateMap<PaymentMethod, PaymentMethodDto>();
            CreateMap<PaymentMethodDto, PaymentMethod>();
        }
    }
}
