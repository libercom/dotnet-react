using Project.Models;

namespace Project.Services
{
    public class DtoMappingService : IDtoMappingService
    {
        public OrderDto OrderToDto(Order order) => new OrderDto
            {
                OrderId = order.OrderId,
                User = UserToDto(order.User),
                ShipmentDate = order.ShipmentDate,
                ArrivalDate = order.ArrivalDate,
                CargoType = order.CargoType,
                Payment = order.Payment,
                PaymentMethod = order.PaymentMethod,
                SendingCountry = order.SendingCountry,
                DestinationCountry = order.DestinationCountry
            };

        public Order DtoToOrder(OrderCreationDto orderDto) => new Order
            {
                OrderId = orderDto.OrderId,
                UserId = orderDto.UserId,
                ShipmentDate = orderDto.ShipmentDate,
                ArrivalDate = orderDto.ArrivalDate,
                SendingCountryId = orderDto.SendingCountryId,
                DestinationCountryId = orderDto.DestinationCountryId,
                CargoTypeId = orderDto.CargoTypeId,
                Payment = orderDto.Payment,
                PaymentMethodId = orderDto.PaymentMethodId
            };

        public UserDto UserToDto(User user) => new UserDto
        {
               UserId = user.UserId,
               FirstName = user.FirstName,
               LastName = user.LastName,
               Email = user.Email,
               PhoneNumber = user.PhoneNumber,
               Company = user.Company,
               Role = user.Role,
               Country = user.Country
           };
        public User DtoToUser(UserCreationDto userDto) => new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                CompanyId = userDto.CompanyId,
                RoleId = userDto.RoleId,
                CountryId = userDto.CountryId
            };
    }
}
