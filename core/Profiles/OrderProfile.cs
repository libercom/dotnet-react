using AutoMapper;
using core.Dtos;
using domain.Models;

namespace core.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderCreationDto, Order>();
        }
    }
}
