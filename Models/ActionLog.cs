namespace RRS.Models
{
	public class ActionLog
	{
		public int Id { get; set; }
		public int ReservationId { get; set; }

		// Action Details
		public string Action { get; set; } // Example: Modify, Cancel, Confirm
		public DateTime ActionDate { get; set; } = DateTime.UtcNow;
		public string Notes { get; set; } // Optional details about the action

		// Tracking
		public string PerformedBy { get; set; } // Admin or Customer
		public string? AdminUser { get; set; } // Optional: Admin username
		public string? CustomerEmail { get; set; } // Optional: Customer email

		// Navigation Property
		public Reservation Reservation { get; set; }
	}
}