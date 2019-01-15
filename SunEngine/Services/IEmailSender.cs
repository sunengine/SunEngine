using System.Threading.Tasks;

namespace SunEngine.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage, string textMessage = null);
    }
}