using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orders.Application.Contracts.Models.EmailService;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        private EmailSettings emailSettings { get; }
        private ILogger<EmailService> logger { get; }

        public EmailService(IOptions<EmailSettings> emailOptions, ILogger<EmailService> logger)
        {
            this.emailSettings = emailOptions?.Value ?? throw new ArgumentNullException(nameof(emailOptions));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SendMailAsync(Email email)
        {
            var client = new SendGridClient(emailSettings.ApiKey);

            var subject = email.Subject;
            var to = new EmailAddress(email.To);
            var emailBody = email.Body;

            var from = new EmailAddress
            {
                Email = emailSettings.FromAddress,
                Name = emailSettings.FromName
            };

            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
            var response = await client.SendEmailAsync(sendGridMessage);

            logger.LogInformation("Email sent.");

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;

            logger.LogError("Email sending failed.");
            return false;
        }
    }
}
