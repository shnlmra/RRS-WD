using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using RRS.Controllers;
using RRS.Models;

namespace RRS.Services
{
	public class PayMongoService
	{
		private readonly HttpClient _httpClient;
		private readonly string _secretKey;
		private readonly string _redirectUrl;
		//private const string BaseUrl = "https://api.paymongo.com/v1/";

		public PayMongoService(IConfiguration configuration)
		{
			_secretKey = configuration["PayMongo:ApiKey"];
			_redirectUrl = configuration["PayMongo:redirectUrl"];
			_httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_secretKey}:"))}");
		}

		public async Task<string> CreatePaymentIntentAsync(PaymentModel payment)
		{
			try
			{
				var client = new HttpClient();
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("https://api.paymongo.com/v1/payment_intents"),
					Headers =
					{
						{ "accept", "application/json" },
						{ "authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_secretKey}:"))}" },
					},
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						data = new
						{
							attributes = new
							{
								amount = (int)(payment.Amount * 100), // Convert to centavos
								payment_method_allowed = new[] { "card", "paymaya", "gcash"},
								payment_method_options = new
								{
									card = new
									{
										request_three_d_secure = "any"
									}
								},
								currency = "PHP",
								capture_type = "automatic",
								description = payment.Description
							}
						}
					}))
					{
						Headers =
						{
							ContentType = new MediaTypeHeaderValue("application/json")
						}
					}
				};

				using (var response = await client.SendAsync(request))
				{
					response.EnsureSuccessStatusCode();
					var body = await response.Content.ReadAsStringAsync();
					var result = JObject.Parse(body);
					var paymentIntentId = result["data"]["id"].ToString();

					return paymentIntentId; // Return the PaymentIntent ID

					//// Create the payment method and attach it to the payment intent, returning the last method's response
					//var paymentMethodCreationResult = await CreatePaymentMethodAsync(paymentIntentId);
					//return paymentMethodCreationResult; // Return the response from the AttachPaymentMethodToIntent method
				}
			}
			catch (Exception ex)
			{
				return $"Error: {ex.Message}";
			}
		}

		public async Task<string> CreatePaymentMethodAsync(PaymentModel payment)
		{
			try
			{
				var client = new HttpClient();
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("https://api.paymongo.com/v1/payment_methods"),
					Headers =
					{
						{ "accept", "application/json" },
						{ "authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_secretKey}:"))}" },
					},
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						data = new
						{
							attributes = new
							{
								details = new
								{
									card_number = payment.Card_number,
									exp_month = payment.Exp_month,
									exp_year = payment.Exp_year,
									cvc = payment.Cvc
								},
								billing = new
								{
									name = payment.Name,
									email = payment.Email,
									phone = payment.Phone
								},
								type = payment.PaymentMethod
							}
						}
					}))

					{
						Headers =
						{
							ContentType = new MediaTypeHeaderValue("application/json")
						}
					}
				};

				Console.WriteLine("Request Payload:");
				Console.WriteLine(JsonConvert.SerializeObject(new
				{
					data = new
					{
						attributes = new
						{
							details = new
							{
								card_number = payment.Card_number,
								exp_month = payment.Exp_month,
								exp_year = payment.Exp_year,
								cvc = payment.Cvc
							},
							billing = new
							{
								name = payment.Name,
								email = payment.Email,
								phone = payment.Phone
							},
							type = payment.PaymentMethod
						}
					}
				}));


				using (var response = await client.SendAsync(request))
				{
					response.EnsureSuccessStatusCode();
					if (!response.IsSuccessStatusCode)
					{
						var errorBody = await response.Content.ReadAsStringAsync();
						Console.WriteLine($"Error response body: {errorBody}");
						return "Failed to create payment method.";
					}
					var body = await response.Content.ReadAsStringAsync();
					var result = JObject.Parse(body);
					var paymentMethodCreated = result["data"] != null && result["data"]["id"] != null;

					if (paymentMethodCreated)
					{
						var paymentMethodId = result["data"]["id"].ToString();
						//var attachResult = await AttachPaymentMethodToIntent(paymentIntentId, paymentMethodId);
						return paymentMethodId; // Return the response from the AttachPaymentMethodToIntent method
					}
					else
					{
						return "Payment method creation failed";
					}
				}
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Request failed: {ex.Message}");
				if (ex.Data != null)
				{
					Console.WriteLine("Error Details: " + ex.Data);
				}
				return "Request failed with an exception.";
			}
		}

		public async Task<string> AttachPaymentMethodToIntent(PaymentModel payment)
		{
			try
			{
				var client = new HttpClient();
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri($"https://api.paymongo.com/v1/payment_intents/{payment.PaymentIntentId}/attach"),
					Headers =
					{
						{ "accept", "application/json" },
						{ "authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_secretKey}:"))}" },
					},
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						data = new
						{
							attributes = new
							{
								payment_method = payment.PaymentMethodId,
								return_url = _redirectUrl
							}
						}
					}))
					{
						Headers =
						{
							ContentType = new MediaTypeHeaderValue("application/json")
						}
					}
				};

				using (var response = await client.SendAsync(request))
				{
					response.EnsureSuccessStatusCode();
					var body = await response.Content.ReadAsStringAsync();
					var result = JObject.Parse(body);
					var paymentCreated = result["data"] != null && result["data"]["id"] != null;

					if (paymentCreated)
					{
						var paymentId = result["data"]["attributes"]["payments"][0]["id"].ToString();
						//var attachResult = await AttachPaymentMethodToIntent(paymentIntentId, paymentMethodId);
						return paymentId; // Return the response from the AttachPaymentMethodToIntent method
					}
					else
					{
						return "Payment method creation failed";
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error attaching payment method to intent: {ex.Message}");
				return "false";
			}
		}

		public async Task<string> CreatePaymentMethodEWAsync(PaymentModel payment)
		{
			try
			{
				var client = new HttpClient();
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("https://api.paymongo.com/v1/payment_methods"),
					Headers =
					{
						{ "accept", "application/json" },
						{ "authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_secretKey}:"))}" },
					},
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						data = new
						{
							attributes = new
							{
								billing = new
								{
									name = payment.Name,
									email = payment.Email,
									phone = payment.Phone
								},
								type = payment.PaymentMethod
							}
						}
					}))
					{
						Headers =
						{
							ContentType = new MediaTypeHeaderValue("application/json")
						}
					}
				};

				using (var response = await client.SendAsync(request))
				{
					response.EnsureSuccessStatusCode();
					if (!response.IsSuccessStatusCode)
					{
						var errorBody = await response.Content.ReadAsStringAsync();
						Console.WriteLine($"Error response body: {errorBody}");
						return "Failed to create payment method.";
					}
					var body = await response.Content.ReadAsStringAsync();
					var result = JObject.Parse(body);
					var paymentMethodCreated = result["data"] != null && result["data"]["id"] != null;

					if (paymentMethodCreated)
					{
						var paymentMethodId = result["data"]["id"].ToString();
						//var attachResult = await AttachPaymentMethodToIntent(paymentIntentId, paymentMethodId);
						return paymentMethodId; // Return the response from the AttachPaymentMethodToIntent method
					}
					else
					{
						return "Payment method creation failed";
					}
				}
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Request failed: {ex.Message}");
				if (ex.Data != null)
				{
					Console.WriteLine("Error Details: " + ex.Data);
				}
				return "Request failed with an exception.";
			}
		}

		public async Task<string> AttachPaymentMethodToIntentEW(PaymentModel payment)
		{
			try
			{
				var client = new HttpClient();
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri($"https://api.paymongo.com/v1/payment_intents/{payment.PaymentIntentId}/attach"),
					Headers =
					{
						{ "accept", "application/json" },
						{ "authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_secretKey}:"))}" },
					},
					Content = new StringContent(JsonConvert.SerializeObject(new
					{
						data = new
						{
							attributes = new
							{
								payment_method = payment.PaymentMethodId,
								return_url = _redirectUrl
							}
						}
					}))
					{
						Headers =
						{
							ContentType = new MediaTypeHeaderValue("application/json")
						}
					}
				};

				using (var response = await client.SendAsync(request))
				{
					response.EnsureSuccessStatusCode();
					var body = await response.Content.ReadAsStringAsync();
					var result = JObject.Parse(body);
					var paymentCreated = result["data"] != null && result["data"]["id"] != null;

					if (paymentCreated)
					{
						// get the payments id
						//var paymentId = result["data"]?["attributes"]?["payments"]?[0]?["id"]?.ToString();

						// get the redirect url
						var redirectUrl = result["data"]["attributes"]["next_action"]?["redirect"]?["url"]?.ToString();

						// return both paymentId
						return redirectUrl;

						// check the status of payments id

					}
					else
					{
						return "Payment method creation failed";
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error attaching payment method to intent: {ex.Message}");
				return "false";
			}
		}

		public async Task<string> RetrievePaymentIntent(string paymentIntentId)
		{
			try
			{
				var client = new HttpClient();
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri($"https://api.paymongo.com/v1/payment_intents/{paymentIntentId}"),
					Headers =
					{
						{ "accept", "application/json" },
						{ "authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_secretKey}:"))}" },
					}
				};

				using (var response = await client.SendAsync(request))
				{
					response.EnsureSuccessStatusCode();
					var body = await response.Content.ReadAsStringAsync();
					var result = JObject.Parse(body);

					var payments = result["data"]?["attributes"]?["payments"];

					var status = result["data"]?["attributes"]?["payments"]?[0]?["attributes"]?["status"]?.ToString();

					// return payment

					return status;

					if (payments != null && payments.HasValues)
					{
						if (status == "failed")
						{
							Console.WriteLine("A payment has failed.");
						}
						else if (status == "paid")
						{
							Console.WriteLine("A payment has been paid.");
						}
						return status;
					}
					else
					{
						// No payments exist
						Console.WriteLine("No payments found.");
						return status;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error attaching payment method to intent: {ex.Message}");
				return "false";
			}

		}

	}
}