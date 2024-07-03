using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EcommerceApplication.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;

        public EmailService(IConfiguration configuration)
        {
            _fromAddress = configuration["EmailSettings:FromAddress"];
            _smtpClient = new SmtpClient
            {
                Host = configuration["EmailSettings:Host"],
                Port = int.Parse(configuration["EmailSettings:Port"]),
                EnableSsl = bool.Parse(configuration["EmailSettings:EnableSsl"]),
                Credentials = new NetworkCredential(
                    configuration["EmailSettings:Username"],
                    configuration["EmailSettings:Password"])
            };
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mailMessage = new MailMessage(_fromAddress, to, subject, body);
            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
