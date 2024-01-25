using YouBank24.Models;

namespace YouBank24.Services.IServices
{
    public interface IEmailCustomEvent
    {
        event EmailSentEventHandler EmailSent;
        void OnEmailSent();
    }
}
