﻿using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using PustokApp.Settings;
using System.Security.Policy;

namespace PustokApp.Services
{
    public class EmailService
    {
        public void SendEmail(string to, string subject, string body,EmailSetting emailSetting)
        {
            //create email  
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailSetting.FromEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            //send email
            email.Body = new TextPart(TextFormat.Html) { Text = body };
            using var smtp = new SmtpClient();
            smtp.Connect(emailSetting.SmtpServer, emailSetting.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSetting.FromEmail , emailSetting.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
