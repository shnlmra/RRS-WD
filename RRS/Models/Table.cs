namespace RRS.Models
{
    public class Table
    {
        public int Id { get; set; }
        public string TableNumber { get; set; }
        public int SeatingCapacity { get; set; }
        public string Location { get; set; }
        public string AdditionalNotes { get; set; }
        public string Status { get; set; } // Status property
    }
}
