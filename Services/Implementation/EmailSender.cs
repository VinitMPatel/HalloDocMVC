using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Services.Contracts;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmail(string email, string subject, string message)
        {
            // Set up SMTP client
            //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            //client.EnableSsl = true;
            //client.UseDefaultCredentials = false;
            //client.Credentials = new NetworkCredential("tatva.dotnet.vinitpatel@outlook.com", "016@ldce");

            // Create email message
            //MailMessage mailMessage = new MailMessage();
            //mailMessage.From = new MailAddress("abby.wehner53@ethereal.email");
            //mailMessage.To.Add(toEmail);
            //mailMessage.Subject = subject;
            //mailMessage.IsBodyHtml = true;
            //StringBuilder mailBody = new StringBuilder();
            //mailBody.AppendFormat("<h1>User Registered</h1>");
            //mailBody.AppendFormat("<br />");
            //mailBody.AppendFormat("<p>Thank you For Registering account</p>");
            //mailMessage.Body = mailBody.ToString();

            // Send email
            //client.Send(mailMessage);

            var emailToSend = new MimeMessage();

            emailToSend.From.Add(MailboxAddress.Parse("vinit.patel@etatvasoft.com"));




            
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };


            using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
            {
                emailClient.Connect("mail.etatvasoft.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("vinit.patel@etatvasoft.com", "e[&OG63ab6_X");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
                return Task.CompletedTask;
            }
        }
    }
}
