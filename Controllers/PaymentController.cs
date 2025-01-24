using Microsoft.AspNetCore.Mvc;
using RRS.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;


namespace RRS.Controllers
{
	public class PaymentController : Controller
	{
		private readonly PayMongoService _payMongoService;
		private readonly IConfiguration _configuration;

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
				// Create payment intent
				var paymentIntentId = await _payMongoService.CreatePaymentIntentAsync(amount);

				// Attach payment method
				//var paymentSuccessful = await _payMongoService.AttachPaymentMethodToIntent(paymentIntentId, paymentMethodId);

				//if (paymentSuccessful)
				//{
				//	return Json(new { success = true, message = "Payment processed successfully" });
				//}

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
		public async Task<IActionResult> TestPayment(decimal amount)
		{
			try
			{
				// Create a test payment intent
				var paymentIntentId = await _payMongoService.CreatePaymentIntentAsync(amount);

				return Ok(new
				{
					resultJson = paymentIntentId,
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
	}
}
