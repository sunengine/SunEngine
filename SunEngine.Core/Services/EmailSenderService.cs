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
        private readonly EmailSenderOptions options;
        private readonly MailTemplatesCache mailTemplatesCache;

        public EmailSenderService(IOptions<EmailSenderOptions> optionsAccessor)
        {
            options = optionsAccessor.Value;
            mailTemplatesCache = new MailTemplatesCache();
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

        protected async Task SendEmailAsync(string toEmail, string subject, string htmlMessage,
            string textMessage = null)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(options.EmailFromAddress, options.EmailFromName),
                Body = textMessage,
                BodyEncoding = Encoding.UTF8,
                Subject = subject,
                SubjectEncoding = Encoding.UTF8
            };
            mailMessage.To.Add(toEmail);


            if (!string.IsNullOrEmpty(htmlMessage))
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlMessage);
                htmlView.ContentType = new ContentType("text/html");
                mailMessage.AlternateViews.Add(htmlView);
            }


            using (SmtpClient client = new SmtpClient(options.Host, options.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(options.Login, options.Password),
                EnableSsl = options.UseSSL
            })
            {
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
