using Ipr.WaterSensor.Server.Properties;
using MailKit.Net.Smtp;
using MimeKit;

namespace Ipr.WaterSensor.Server.Services
{
    public class EmailService
    {
        public string Header { get; set; }
        public string Body { get; set; }

        private void SendMail(string name, string emailAddress)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Regenwaterput", emailAddress));
            email.To.Add(new MailboxAddress(name, emailAddress));
            email.Subject = Header;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = Body
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(Resources.SMTPServer, int.Parse(Resources.SMTPPort), MailKit.Security.SecureSocketOptions.None);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }

    }
}

