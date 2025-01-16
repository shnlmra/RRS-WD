namespace RRS.Models
{
    public class Reservation
    {
        public string Time { get; set; }
        public string Name { get; set; }
        public int Guests { get; set; }
        public int Table { get; set; }
        public required string Status { get; set; }
    }
}