using domain.Models;

namespace common.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public UserDto User { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public CargoType CargoType { get; set; }
        public int Payment { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Country SendingCountry { get; set; }
        public Country DestinationCountry { get; set; }
    }
}
