namespace YouBank24.Models.ViewModels {
    public class TransactionDetailsViewModel {
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
        public string ReceiverEmail { get; set; }
        public float Amount { get; set; }
        public string Timestamp { get; set; }
        public string Note { get; set; }
        public Transaction Transaction { get; set; }
        public Account Account { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
