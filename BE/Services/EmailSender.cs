using System.Threading.Tasks;
using BE.Config;
using BE.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BE.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;

        public EmailSender(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            Console.WriteLine("Sending email");
            Console.WriteLine(_settings.SenderEmail);

            // From
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            // To
            message.To.Add(new MailboxAddress("", to));
            // Subject
            message.Subject = subject;

            // Body
            message.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                // Connect
                await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, _settings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto);

                // Authenticate
                await client.AuthenticateAsync(_settings.Username, _settings.Password);

                // Send
                await client.SendAsync(message);

                // Disconnect
                await client.DisconnectAsync(true);
            }
            Console.WriteLine("Email sent");
        }
    }
}
