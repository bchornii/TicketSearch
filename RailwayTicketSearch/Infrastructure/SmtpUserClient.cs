using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RailwayTicketSearch.Infrastructure
{
    public class SmtpUserClient
    {
        public async Task SendEmail(string subject, string body)
        {
            var appSettings = new AppSettings().GetAppSettings();
            var from = new MailAddress(appSettings.EmailFrom, "Ticket Client");
            var to = new MailAddress(appSettings.EmailTo);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(appSettings.EmailFrom, appSettings.EmailPassword)
            };
            await smtp.SendMailAsync(new MailMessage(from, to)
            {
                Subject = subject,
                Body = body
            });
        }
    }
}
