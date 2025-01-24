namespace RRS.Models
{
	public class Payment
	{
		// reference to the reservation
		public int ReservationId { get; set; }
		public decimal Amount { get; set; } // Payment amount
		public string Description { get; set; } // Description of the payment
		public string PaymentMethod { get; set; } // E.g., "Credit Card", "PayPal"
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp for the payment

		public string PaymentIntentId { get; set; } // Payment Intent ID from PayMongo or another service
		public string PaymentMethodId { get; set; } // Payment Method ID from PayMongo or another service
		public string PaymentId { get; set; } // Payment ID from PayMongo or another service
		public string Status { get; set; } = "Pending"; // Default status

		// Navigation property
		public Reservation Reservation { get; set; }
	}
}
