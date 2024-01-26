using Ipr.WaterSensor.Core.Entities;
using Ipr.WaterSensor.Infrastructure.Data;
using Ipr.WaterSensor.Server.Properties;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using ChartJs.Blazor.Common;
using System.Text.RegularExpressions;

namespace Ipr.WaterSensor.Server.Services
{
    public class EmailService
    {
        public string Header { get; set; }
        public string Body { get; set; }

        public void SendMail(string name, string emailAddress, string topic)
        {
            MailMessage email = new MailMessage();
            email.From = new MailAddress(Resources.SMTPUsername);
            email.To.Add(emailAddress);

            if (topic == Resources.MQTTTopicBatteryLevel)
            {
                email.Subject = Resources.BatteryAlarm_header;
                email.Body = Resources.BatteryAlarm_body;
            }

            if (topic == Resources.MQTTTopicMainTank)
            {
                email.Subject = Resources.WaterLevelAlarm_header;
                email.Body = Resources.WaterLevelAlarm_body;
            }

            email.Body = InsertName(email.Body, name);

            ICredentialsByHost credentials = new NetworkCredential(Resources.SMTPUsername, Resources.SMTPPassword);

            using (var smtp = new SmtpClient())
            {
                smtp.Host = Resources.SMTPServer;
                smtp.Port = int.Parse(Resources.SMTPPort);
                smtp.EnableSsl = true;
                smtp.Credentials = credentials;
                smtp.Send(email);
            }
        }

        private string InsertName(string body, string name)
        {
            var regex = new Regex("{{(.*?)}}");
            var match = regex.Match(body);
            body = body.Replace(match.Value, name);
            return body;
        }
    }
}

