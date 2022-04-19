namespace core.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int CargoTypeId { get; set; }
        public virtual CargoType CargoType { get; set; }
        public int Payment { get; set; }
        public int PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public int SendingCountryId { get; set; }
        public virtual Country SendingCountry { get; set; }
        public int DestinationCountryId { get; set; }
        public virtual Country DestinationCountry { get; set; }
    }
}
