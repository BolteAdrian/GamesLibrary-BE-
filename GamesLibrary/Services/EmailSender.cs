using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using GamesLibrary.DataAccessLayer.Contacts;
using GamesLibrary.Utils.Constants;

namespace GamesLibrary.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig?.Value ?? throw new ArgumentNullException(nameof(emailConfig), ResponseConstants.EMAIL.CONFIG_NULL);
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), ResponseConstants.EMAIL.EMAIL_NULL_OR_EMPTY);
            }

            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailConfig.FromName, _emailConfig.FromAddress));
                emailMessage.To.Add(new MailboxAddress("Recipient Name", email)); // Provide the recipient name here
                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = message;
                emailMessage.Body = bodyBuilder.ToMessageBody();

                using var smtpClient = new SmtpClient();
                smtpClient.Connect(_emailConfig.SmtpServer, _emailConfig.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                smtpClient.Authenticate(_emailConfig.SmtpUsername, _emailConfig.SmtpPassword);
                await smtpClient.SendAsync(emailMessage);
                smtpClient.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ResponseConstants.EMAIL.ERROR_SENDING, ex);
            }
        }
    }
}
