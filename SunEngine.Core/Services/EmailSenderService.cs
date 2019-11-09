using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Core.Services
{
    public interface IEmailSenderService
    {
        Task SendEmailByTemplateAsync(
            string toEmail,
            string templateName,
            Dictionary<string, string> replaceDictionary
        );
    }

    public class EmailSenderService : IEmailSenderService
    {
        private readonly IOptionsSnapshot<EmailSenderOptions> emailSenderOptions;
        private readonly IMailTemplatesCache mailTemplatesCache;

        public EmailSenderService(
            IOptionsSnapshot<EmailSenderOptions> emailSenderOptions,
            IMailTemplatesCache mailTemplatesCache)
        {
            this.emailSenderOptions = emailSenderOptions;
            this.mailTemplatesCache = mailTemplatesCache;
        }

        public async Task SendEmailByTemplateAsync(
            string toEmail,
            string templateName,
            Dictionary<string, string> replaceDictionary
        )
        {
            var message = mailTemplatesCache.BuildMessage(templateName, replaceDictionary);
            await SendEmailAsync(toEmail, message.subject, message.template);
        }

        protected async Task SendEmailAsync(
            string toEmail, string subject, string htmlMessage, string textMessage = null)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(emailSenderOptions.Value.EmailFromAddress, emailSenderOptions.Value.EmailFromName),
                Body = textMessage,
                Subject = subject,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };
            mailMessage.To.Add(toEmail);

            if (!string.IsNullOrEmpty(htmlMessage))
            {
                var htmlView = AlternateView.CreateAlternateViewFromString(htmlMessage);
                htmlView.ContentType = new ContentType("text/html");
                mailMessage.AlternateViews.Add(htmlView);
            }
            
            using (SmtpClient client = new SmtpClient(emailSenderOptions.Value.Host, emailSenderOptions.Value.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailSenderOptions.Value.Login, emailSenderOptions.Value.Password),
                EnableSsl = emailSenderOptions.Value.UseSSL
            })
            {
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
