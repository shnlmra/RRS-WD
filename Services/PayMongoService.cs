using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace RRS.Services
{
	public class PayMongoService
	{
		private readonly HttpClient _httpClient;
		private readonly string _secretKey;
		//private const string BaseUrl = "https://api.paymongo.com/v1/";

		public PayMongoService(IConfiguration configuration)
		{
			_secretKey = configuration["PayMongo:ApiKey"];
			_httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_secretKey}:"))}");
		}

		public async Task<string> CreatePaymentIntentAsync(decimal amount, string description = "Test Payment")
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
								amount = (int)(amount * 100), // Convert to centavos
								payment_method_allowed = new[] { "qrph", "card", "dob", "paymaya", "billease", "gcash", "grab_pay" },
								payment_method_options = new
								{
									card = new
									{
										request_three_d_secure = "any"
									}
								},
								currency = "PHP",
								capture_type = "automatic",
								description = "Payment for <> reservation"
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

					// Create the payment method and attach it to the payment intent, returning the last method's response
					var paymentMethodCreationResult = await CreatePaymentMethodAsync(paymentIntentId);
					return paymentMethodCreationResult; // Return the response from the AttachPaymentMethodToIntent method
				}
			}
			catch (Exception ex)
			{
				return $"Error: {ex.Message}";
			}
		}

		public async Task<string> CreatePaymentMethodAsync(string paymentIntentId)
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
									card_number = "4343434343434345",
									exp_month = 12,
									exp_year = 30,
									cvc = "111"
								},
								billing = new
								{
									address = new
									{
										line1 = "123 Main St",
										city = "Quezon City",
										state = "Metro Manila",
										postal_code = "1111",
										country = "PH"
									},
									name = "eRICE",
									email = "ericemarial@gmail.com",
									phone = "09560429604"
								},
								type = "card"
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
					var paymentMethodCreated = result["data"] != null && result["data"]["id"] != null;

					if (paymentMethodCreated)
					{
						var paymentMethodId = result["data"]["id"].ToString();
						var attachResult = await AttachPaymentMethodToIntent(paymentIntentId, paymentMethodId);
						return attachResult; // Return the response from the AttachPaymentMethodToIntent method
					}
					else
					{
						return "Payment method creation failed";
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error creating payment method: {ex.Message}");
				return "Error creating payment method";
			}
		}

		public async Task<string> AttachPaymentMethodToIntent(string paymentIntentId, string paymentMethodId)
		{
			try
			{
				var client = new HttpClient();
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri($"https://api.paymongo.com/v1/payment_intents/{paymentIntentId}/attach"),
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
								payment_method = paymentMethodId,
								return_url = "https://github.com/dnsxmrs/fiweb"
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
					return body; // Return the response body as a string
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error attaching payment method to intent: {ex.Message}");
				return "false";
			}
		}

		//private async Task<JObject> SendPostRequestAsync(string endpoint, object payload)
		//{
		//	try
		//	{
		//		var jsonPayload = JsonConvert.SerializeObject(payload);
		//		var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

		//		var response = await _httpClient.PostAsync($"{BaseUrl}{endpoint}", content);

		//		// Read response content for detailed error information
		//		var responseContent = await response.Content.ReadAsStringAsync();

		//		if (!response.IsSuccessStatusCode)
		//		{
		//			// Log or throw a more informative exception
		//			throw new HttpRequestException($"PayMongo API Error: {response.StatusCode} - {responseContent}");
		//		}

		//		return JObject.Parse(responseContent);
		//	}
		//	catch (Exception ex)
		//	{
		//		// Log the full exception details
		//		throw new Exception($"PayMongo API Call Failed: {ex.Message}", ex);
		//	}
		//}
	}
}