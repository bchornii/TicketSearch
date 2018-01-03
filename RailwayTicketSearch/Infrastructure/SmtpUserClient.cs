using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RailwayTicketSearch.Infrastructure
{
    public class SmtpUserClient
    {
        public async Task SendEmail(string subject, string body)
        {
            var from = new MailAddress("ticketsearchclient@gmail.com", "Ticket Client");
            var to = new MailAddress("bogdan.chorniy@gmail.com");
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("ticketsearchclient@gmail.com", "QAZwsx123")
            };
            await smtp.SendMailAsync(new MailMessage(from, to)
            {
                Subject = subject,
                Body = body
            });
        }
    }
}
