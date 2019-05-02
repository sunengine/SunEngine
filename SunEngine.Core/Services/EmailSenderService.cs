using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Core.Services
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage, string textMessage = null);
    }
    
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailSenderOptions options;

        public EmailSenderService(IOptions<EmailSenderOptions> optionsAccessor)
        {
            options = optionsAccessor.Value;
        }


        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage, string textMessage = null)
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
                htmlView.ContentType = new System.Net.Mime.ContentType("text/html");
                mailMessage.AlternateViews.Add(htmlView);
            }


            using (SmtpClient client = new SmtpClient(options.Host, options.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(options.Login, options.Password),
                EnableSsl = options.UseSSL
            })
            {
                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch(Exception e)
                {
                    
                }
            }
        }
    }
}