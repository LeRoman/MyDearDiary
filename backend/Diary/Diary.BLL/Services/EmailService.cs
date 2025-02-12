using Diary.BLL.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Diary.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection emailSettings;
        private readonly string smtpServer;
        private readonly int port;
        private readonly string username;
        private readonly string password;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            emailSettings = _configuration.GetSection("EmailSettings");
            smtpServer = emailSettings["SmtpServer"]!;
            port = int.Parse(emailSettings["Port"]!);
            username = emailSettings["Username"]!;
            password = emailSettings["Password"]!;
        }

        public async Task SendAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("MyDiary", username));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(smtpServer, port);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
