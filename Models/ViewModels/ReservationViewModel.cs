namespace RRS.Models.ViewModels
{
    public class ReservationViewModel
    {
        public Reservation Reservation { get; set; }
        public Table Table { get; set; }
        public Customer Customer { get; set; }

        public List<Reservation> reservationsToday { get; set; }
        public List<Reservation> UpcommingReservations { get; set; }
    }
}
