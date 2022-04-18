using Project.Models;

namespace Project.Services
{
    public interface IDtoMappingService
    {
        public User DtoToUser(UserCreationDto userDto);
        public Order DtoToOrder(OrderCreationDto userDto);
        public UserDto UserToDto(User user);
        public OrderDto OrderToDto(Order order);
    }
}
