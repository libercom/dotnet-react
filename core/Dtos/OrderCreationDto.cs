namespace core.Models
{
    public class OrderCreationDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int CargoTypeId { get; set; }
        public int Payment { get; set; }
        public int PaymentMethodId { get; set; }
        public int SendingCountryId { get; set; }
        public int DestinationCountryId { get; set; }
    }
}
