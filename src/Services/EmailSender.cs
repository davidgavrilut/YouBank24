using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using YouBank24.Models;
using YouBank24.Services.IServices;

namespace YouBank24.Services;
public class EmailSender : IEmailSender, IEmailCustomEvent {
    private EmailSentEventHandler _emailSentEventHandlers;
    private readonly EmailConnectionOptions _emailConnectionOptions;

    public EmailSender(IConfiguration configuration)
    {
        _emailConnectionOptions = configuration.GetSection("EmailConnection").Get<EmailConnectionOptions>(); ;
    }

    public event EmailSentEventHandler EmailSent
    {
        add => _emailSentEventHandlers += value;
        remove => _emailSentEventHandlers -= value;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage) {
        var emailToSend = new MimeMessage();
        emailToSend.From.Add(MailboxAddress.Parse(_emailConnectionOptions.EmailFrom));
        emailToSend.To.Add(MailboxAddress.Parse(email));
        emailToSend.Subject = subject;
        emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = htmlMessage
        };

        using (var emailClient = new SmtpClient())
        {
            emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            emailClient.Authenticate(_emailConnectionOptions.ConnectionEmailAddress, _emailConnectionOptions.ConnectionPassword);
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);
        }

        OnEmailSent();

        return Task.CompletedTask;
    }

    public virtual void OnEmailSent()
    {
        _emailSentEventHandlers?.Invoke(this, new EmailSentEventArgs());
        Console.WriteLine("<<Email Sent>> event raised");
    }
}