namespace RRS.Models.ViewModels
{
    public class ReservationViewModel
    {
        public Reservation Reservation { get; set; }
        public Table Table { get; set; }
        public Customer Customer { get; set; }
        public List<Table> Tables { get; set; }
        public List<Customer> Customers { get; set; }

        public List<BuffetType> BuffetTypes { get; set; }

    }
}
