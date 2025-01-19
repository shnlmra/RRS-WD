namespace RRS.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public DateOnly ReservationDate { get; set; }
        public TimeOnly ReservationTime { get; set; }
        public int NumberOfGuest { get; set; }
        public string OccasionType { get; set; }  
        public string RestaurantBranch { get; set; }
        public string? SpecialRequest { get; set; }
        public string Status { get; set; }


        // Foreign key for table
        public int TableId { get; set; }
        public Table Table { get; set; }

        // Foreign key for menu
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        // Foreign key for customer
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // Enum for Occasion Type
    //public enum OccasionType
    //{
    //    Birthday,   // Birthday occasion
    //    Anniversary, // Anniversary occasion
    //    Casual,     // Casual dining
    //    Business,   // Business event or meeting
    //    Celebration, // General celebration
    //    Other       // For any other type of occasion
    //}

    //// Enum for Reservation Status
    //public enum ReservationStatus
    //{
    //    Pending,   // Reservation is still pending
    //    Confirmed, // Reservation has been confirmed
    //    Cancelled, // Reservation has been cancelled
    //    Completed  // Reservation is completed
    //}

}