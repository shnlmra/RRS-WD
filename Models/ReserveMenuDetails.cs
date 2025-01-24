namespace RRS.Models
{
    public class ReserveMenuDetails
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int ReservationId { get; set; }


        public Menu Menu { get; set; }
        public Reservation Reservation { get; set; }
    }
}
