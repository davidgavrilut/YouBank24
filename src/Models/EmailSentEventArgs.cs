namespace YouBank24.Models
{
    public class EmailSentEventArgs : EventArgs
    {
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public DateTime SendingDate { get; set; }
    }

    public delegate void EmailSentEventHandler(object sender, EmailSentEventArgs e);
}
