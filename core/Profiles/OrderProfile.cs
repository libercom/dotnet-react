using AutoMapper;
using common.Dtos;
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
