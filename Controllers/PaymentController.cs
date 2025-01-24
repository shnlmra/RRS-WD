using Microsoft.AspNetCore.Mvc;
using RRS.Services;
using Microsoft.EntityFrameworkCore;
using RRS.Data;
using RRS.Models;
using System.Diagnostics;


namespace RRS.Controllers
{
	public class PaymentController : Controller
	{
		private readonly PayMongoService _payMongoService;
		private readonly IConfiguration _configuration;
		private string paymentIntentId;
		private string paymentMethodId;
		private string paymentId;

		public PaymentController(PayMongoService payMongoService, IConfiguration configuration)
		{
			_payMongoService = payMongoService;
			_configuration = configuration;
		}

		[HttpPost]
		public async Task<IActionResult> ProcessPayment(decimal amount, string paymentMethodId)
		{
			try
			{
				return Json(new { success = false, message = "Payment failed" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

		// Method to expose public key for frontend
		[HttpGet]
		public IActionResult GetPublicKey()
		{
			var publicKey = _configuration["PayMongo:PublicKey"];
			return Json(new { publicKey });
		}

		[HttpPost]
		//public async Task<IActionResult> TestPayment(decimal amount)
		public async Task<IActionResult> TestPayment([FromBody] PaymentModel payment)
		{
			try
			{
				// Validate the incoming payment data
				if (payment == null || payment.Amount <= 0)
				{
					return BadRequest(new { success = false, message = "Invalid payment data." });
				}

				// Create a PaymentIntent using the service (assumes _payMongoService is injected)
				payment.PaymentIntentId = await _payMongoService.CreatePaymentIntentAsync(payment);
				paymentIntentId = payment.PaymentIntentId;

				if (payment.PaymentMethod == "card")
				{
					payment.PaymentMethodId = await _payMongoService.CreatePaymentMethodAsync(payment);
					payment.PaymentId = await _payMongoService.AttachPaymentMethodToIntent(payment);
					// handle card payments
				}
				else
				{
					payment.PaymentMethodId = await _payMongoService.CreatePaymentMethodEWAsync(payment);
					paymentMethodId = payment.PaymentMethodId;

					var redirect = await _payMongoService.AttachPaymentMethodToIntentEW(payment);
					// handles the success or fail payment
					//return Redirect(redirect);  // Properly return the redirect URL
					return Json(new
					{
						success = true,
						message = "Payment method attached successfully.",
						payment.PaymentIntentId,
						payment.PaymentMethodId,
						redirectUrl = redirect // Send the redirect URL as part of the response
					});
				}

				return Json(new
				{
					success = true,
					message = "Payment succeeded",
					payment.PaymentIntentId,
					payment.PaymentMethodId,
					payment.PaymentId
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new
				{
					success = false,
					message = ex.Message
				});
			}
		}

		[HttpGet("payment/callback")]
		public async Task<IActionResult> PaymentCallback()
		{
			try
			{
				// Extract payment_intent_id from query string
				var paymentIntentId = HttpContext.Request.Query["payment_intent_id"].ToString();

				// Validate that the paymentIntentId is not null or empty
				if (string.IsNullOrEmpty(paymentIntentId))
				{
					Console.WriteLine("bad request");
					return BadRequest(new { success = false, message = "Missing payment_intent_id" });
				}

				// You can use this paymentIntentId to retrieve the payment status (optional)
				var status = await _payMongoService.RetrievePaymentIntent(paymentIntentId);

				// If payment details are fetched successfully, proceed to handle different statuses
				if (status != null)
				{
					if (status == "paid")
					{
						// Return the "Success" view for paid payments
						//return View("success", payment);

						return Json(new { success = true, message = "Payment succeeded", paymentIntentId });
					}
					else if (status == "failed")
					{
						Console.WriteLine("A payment has failed.");
						// Handle payment failure
						return Json(new { success = false, message = "Payment failed" });
					}
					else
					{
						Console.WriteLine("A payment has uknown.");
						// Handle other statuses, like pending, awaiting confirmation, etc.
						return Json(new { success = false, message = "Payment status unknown", paymentIntentId });
					}
				}
				else
				{
					return BadRequest(new { success = false, message = "Unable to fetch payment details" });
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("bad request");
				// Log the exception if needed
				return BadRequest(new { success = false, message = ex.Message });
			}
		}
	}
}


