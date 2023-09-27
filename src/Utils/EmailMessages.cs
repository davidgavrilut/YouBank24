namespace YouBank24.Utils
{
    public class EmailMessages
    {
        public const string SenderMessageSubject = "New Transaction Sent";
        public const string ReceiverMessageSubject = "New Transaction Received";

        public static string SenderMessageBody(string senderFirstName, string senderLastName, string receiverFirstName, string receiverLastName, float amount, string note)
        {
            if (note.Length > 0)
            {
                return $"<h3>New Transaction Sent</h3><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p>A new transaction has been successfully created on the YouBank24 platform.</p><p>You, {senderFirstName} {senderLastName}, sent ${amount} to {receiverFirstName} {receiverLastName}.</p></p>You also included the following note: \"{note}\"</p><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p style=\"font-style: italic; font-weight: 600\">YouBank Team</p>";
            } else
            {
                return $"<h3>New Transaction Sent</h3><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p>A new transaction has been successfully created on the YouBank24 platform.</p><p>You, {senderFirstName} {senderLastName}, sent ${amount} to {receiverFirstName} {receiverLastName}.<p><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p style=\"font-style: italic; font-weight: 600\">YouBank Team</p>";
            }
        }

        public static string ReceiverMessageBody(string senderFirstName, string senderLastName, string receiverFirstName, string receiverLastName, float amount, string note)
        {
            if (note.Length > 0) 
            {
                return $"<h3>New Transaction Received</h3><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p>A new transaction has been successfully created on the YouBank24 platform.</p><p>You, {receiverFirstName} {receiverLastName}, have just received ${amount} from {senderFirstName} {senderLastName}.</p><p>The transaction also came with the following note: \"{note}\"</p><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p style=\"font-style: italic; font-weight: 600\">YouBank Team</p>";
            } 
            else
            {
                return $"<h3>New Transaction Received</h3><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p>A new transaction has been successfully created on the YouBank24 platform.</p><p>You, {receiverFirstName} {receiverLastName}, have just received ${amount} from {senderFirstName} {senderLastName}.</p><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p style=\"font-style: italic; font-weight: 600\">YouBank Team</p>";
            }
            
        }

    }
}
