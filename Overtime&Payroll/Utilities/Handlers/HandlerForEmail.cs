using System.Net.Mail;
using System.Net;
using Overtime_Payroll.Contracts;

namespace Overtime_Payroll.Utilities.Handlers
{
    public class HandlerForEmail : IEmailHandler
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromEmailAddress;
        private readonly string _fromEmailPassword;

        public HandlerForEmail(string smtpServer, int smtpPort, string fromEmailAddress, string fromEmailPassword)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _fromEmailAddress = fromEmailAddress;
            _fromEmailPassword = fromEmailPassword;
        }

        public void SendEmail(string toEmail, string subject, string htmlMessage)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_fromEmailAddress),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(toEmail));

            /* using var client = new SmtpClient(_smtpServer, _smtpPort);*/
            var client = new SmtpClient(_smtpServer)
            {
                Port = _smtpPort,
                //UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_fromEmailAddress, _fromEmailPassword),
                EnableSsl = true,
            };
            client.Send(message);
        }
    }
}
