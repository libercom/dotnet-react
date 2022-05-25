using AutoMapper;
using common.Dtos;
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
