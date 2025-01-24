namespace RRS.Controllers
{
	public class PaymentModel
	{

		public decimal Amount { get; set; } //
		public string Description { get; set; } //
		public string PaymentMethod { get; set; }
		public string Name { get; set; }//
		public string Email { get; set; }//
		public string Phone { get; set; }//

		public string PaymentIntentId { get; set; }//
		public string PaymentMethodId { get; set; }//
		public string PaymentId { get; set; }

		// for card payment
		public string Card_number { get; set; } //
		public int Exp_month { get; set; }//
		public int Exp_year { get; set; }//
		public string Cvc { get; set; }//
	}
}