using YouBank24.Models.ViewModels;

namespace YouBank24.Services.IServices
{
    public interface IEmailMessage
    {
        string SenderMessageSubject { get; set; }
        string ReceiverMessageSubject { get; set; }
        string SimulationEmailSubject { get; set; }
        string GenerateSenderEmailBody(NewTransactionCreated newTransaction);
        string GenerateReceiverEmailBody(NewTransactionCreated newTransaction);
        string SimulationEmailBody(string country, int amount, int period, double monthlyPayment, double interest, double totalPayableAmount, string centralBank, string lastUpdated);
    }
}
