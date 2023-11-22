using YouBank24.Models.ViewModels;
using YouBank24.Services.IServices;

namespace YouBank24.Services
{
    public class EmailMessage : IEmailMessage
    {
        public string SenderMessageSubject { get; set; }
        public string ReceiverMessageSubject { get; set; }
        public string SimulationEmailSubject { get; set; }

        public EmailMessage()
        {
            SenderMessageSubject = "New Transaction Sent";
            ReceiverMessageSubject = "New Transaction Received";
            SimulationEmailSubject = "YouBank Simulation Results";
        }

    public string GenerateSenderEmailBody(NewTransactionCreated newTransaction)
        {
            if (newTransaction.Transaction.Note?.Length > 0)
            {
                return $"<h3>New Transaction Sent</h3><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p>A new transaction has been successfully created on the YouBank24 platform.</p><p>You, {newTransaction.SenderUser.FirstName} {newTransaction.SenderUser.LastName}, sent ${newTransaction.Transaction.Amount} to {newTransaction.ReceiverUser.FirstName} {newTransaction.ReceiverUser.LastName}.</p></p>You also included the following note: \"{newTransaction.Transaction.Note}\"</p><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p style=\"font-style: italic; font-weight: 600\">YouBank Team</p>";
            } else
            {
                return $"<h3>New Transaction Sent</h3><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p>A new transaction has been successfully created on the YouBank24 platform.</p><p>You, {newTransaction.SenderUser.FirstName} {newTransaction.SenderUser.LastName}, sent ${newTransaction.Transaction.Amount} to {newTransaction.ReceiverUser.FirstName} {newTransaction.ReceiverUser.LastName}.<p><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p style=\"font-style: italic; font-weight: 600\">YouBank Team</p>";
            }
        }

        public string GenerateReceiverEmailBody(NewTransactionCreated newTransaction)
        {
            if (newTransaction.Transaction.Note?.Length > 0) 
            {
                return $"<h3>New Transaction Received</h3><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p>A new transaction has been successfully created on the YouBank24 platform.</p><p>You, {newTransaction.ReceiverUser.FirstName} {newTransaction.ReceiverUser.LastName}, have just received ${newTransaction.Transaction.Amount} from {newTransaction.SenderUser.FirstName} {newTransaction.SenderUser.LastName}.</p><p>The transaction also came with the following note: \"{newTransaction.Transaction.Note}\"</p><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p style=\"font-style: italic; font-weight: 600\">YouBank Team</p>";
            } 
            else
            {
                return $"<h3>New Transaction Received</h3><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p>A new transaction has been successfully created on the YouBank24 platform.</p><p>You, {newTransaction.ReceiverUser.FirstName} {newTransaction.ReceiverUser.LastName}, have just received ${newTransaction.Transaction.Amount} from {newTransaction.SenderUser.FirstName} {newTransaction.SenderUser.LastName}.</p><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p style=\"font-style: italic; font-weight: 600\">YouBank Team</p>";
            }
        }

        public string SimulationEmailBody(string country, int amount, int period, double monthlyPayment, double interest, double totalPayableAmount, string centralBank, string lastUpdated)
        {
            return $"<h3>YouBank Simulation Results</h3><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p>You have saved the following simulation perform on the YouBank24 platform:</p><p>Simulation done for {country}.</p><p>Requested amount: ${amount}</p><p>Requested period: {period} months</p><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p>Monthly payment: ${monthlyPayment}</p><p>APR: {interest}% - Last updated by {centralBank} on {lastUpdated}</p><p>Total payable amount: ${totalPayableAmount}</p><hr style=\"\r\n    width: 10%;\r\n    margin-left: 0;\r\n\"><p style=\"font-style: italic; font-weight: 600\">YouBank Team</p>";
        }

    }
}
