namespace RRS.Models
{
    public class Table
    {
        public int Id { get; set; }
        public required string TableNumber { get; set; }
        public int SeatingCapacity { get; set; }
        public required string Location { get; set; }
        public string? AdditionalNotes { get; set; }
        public required string Status { get; set; } // Status property
    }
}