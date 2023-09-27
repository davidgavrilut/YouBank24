﻿using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace YouBank24.Utils;
public class EmailSender : IEmailSender {
    public Task SendEmailAsync(string email, string subject, string htmlMessage) {
        var emailToSend = new MimeMessage();
        emailToSend.From.Add(MailboxAddress.Parse("davidgavrilut1@gmail.com"));
        emailToSend.To.Add(MailboxAddress.Parse(email));
        emailToSend.Subject = subject;
        emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = htmlMessage
        };

        // Sending Email
        using (var emailClient = new SmtpClient())
        {
            emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            emailClient.Authenticate("davidgavrilut1@gmail.com", "");
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);
        }
        return Task.CompletedTask;
    }
}