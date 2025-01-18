namespace RRS.Models.Entities
{
	public class ActionLog
	{
		public int Id { get; set; }
		public int ReservationId { get; set; }

		// Action Details
		public string Action { get; set; } // Example: Modify, Cancel, Confirm
		public DateTime ActionDate { get; set; }
		public string? Notes { get; set; } // Optional details about the action (nullable)

		// Tracking
		public string PerformedBy { get; set; } // Could be "Admin" or "Customer"
		public string? AdminUser { get; set; } // Who took the action, specifically for admins (nullable)
		public string? CustomerEmail { get; set; } // If customer performed the action (nullable)

		// Navigation Property
		public Reservation Reservation { get; set; }
	}
}
