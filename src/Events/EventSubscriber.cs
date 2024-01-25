using Microsoft.AspNetCore.Identity.UI.Services;
using YouBank24.Models;
using YouBank24.Services;
using YouBank24.Services.IServices;

namespace YouBank24.Events
{
    public class EventSubscriber
    {
        public void Subscribe(IEmailCustomEvent emailCustomEvent)
        {
            try
            {
                emailCustomEvent.EmailSent += EmailService_EmailSent;
                Console.WriteLine("---> Event is being received");
            } catch
            {
                Console.WriteLine("---> Could not subscribe");
                throw;
            }
        }

        private void EmailService_EmailSent(object sender, EmailSentEventArgs e)
        {
            Console.WriteLine("Email sent event received!");
        }
    }
}
