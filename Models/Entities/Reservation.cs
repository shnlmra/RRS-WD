using Microsoft.EntityFrameworkCore;

namespace RRS.Models.Entities
{
	public class Reservation
	{
		public int Id { get; set; }

		// Reservation Details
		public DateTime ReservationDate { get; set; }
		public TimeSpan ReservationTime { get; set; }
		public int Seats { get; set; }
		public string Buffet { get; set; } // Buffet type or selection

		// Customer Details
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public string PhoneNumber { get; set; }

		// Preferences and Requests
		public string SeatingPreference { get; set; } // Example: Window, Booth, etc.
		public string SpecialRequest { get; set; } // Optional notes or requests

		// Payment Details
		[Precision(18, 2)]
		public decimal TotalAmount { get; set; } // Total cost of the reservation
		[Precision(18, 2)]
		public decimal AmountToPay { get; set; } // Down payment or pre-payment amount

		// Status Tracking
		public string Status { get; set; } // Example: Pending, Confirmed, Cancelled, Modified
		public DateTime CreatedAt { get; set; }
		public DateTime? ModifiedAt { get; set; }
	}

}
