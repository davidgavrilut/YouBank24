namespace YouBank24.Models.ViewModels
{
    public class NewTransactionCreatedModel
    {
        public Transaction Transaction { get; set; }
        public ApplicationUser ReceiverUser { get; set; }
        public ApplicationUser SenderUser { get; set; }

    }
}
