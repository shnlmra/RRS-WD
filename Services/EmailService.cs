using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using RRS.Services;

public class EmailService : IEmailService
{
	private readonly IConfiguration _configuration;

	public EmailService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task SendEmailAsync(string toEmail, string subject, string body)
	{
		var emailSettings = _configuration.GetSection("EmailSettings");

		var message = new MimeMessage();
		message.From.Add(new MailboxAddress(
			emailSettings["SenderName"],
			emailSettings["SenderEmail"]));  // Gmail email
		message.To.Add(new MailboxAddress("", toEmail));  // Customer's email
		message.Subject = subject;

		message.Body = new TextPart("html")
		{
			Text = body
		};

		using (var client = new SmtpClient())
		{
			await client.ConnectAsync(
				emailSettings["SmtpServer"],
				int.Parse(emailSettings["Port"]),
				MailKit.Security.SecureSocketOptions.StartTls);

			await client.AuthenticateAsync(
				emailSettings["Username"],   // Your Gmail username
				emailSettings["Password"]);  // Your Gmail app password

			await client.SendAsync(message);
			await client.DisconnectAsync(true);
		}
	}
}
