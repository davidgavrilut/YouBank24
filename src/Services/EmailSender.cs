using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using YouBank24.Models;
using YouBank24.Services.IServices;

namespace YouBank24.Services;
public class EmailSender : IEmailSender, IEmailCustomEvent {
    public event EmailSentEventHandler EmailSent;
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
            emailClient.Authenticate("davidgavrilut1@gmail.com", "lychkbpshfafanuk");
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);
        }

        OnEmailSent();

        return Task.CompletedTask;
    }

    public virtual void OnEmailSent()
    {
        EmailSent?.Invoke(this, new EmailSentEventArgs());
        Console.WriteLine("---> <<Email Sent>> event raised");
    }
}