using System.ComponentModel.DataAnnotations;

namespace YouBank24.Models; 
public class Transaction {
    [Key]
    public Guid TransactionId { get; set; }
    [Required]
    public float Amount { get; set; }
    [Required]
    public DateTime TransactionTimestamp { get; set; }
    public string TransactionStatus { get; set; }
    [Required]
    public string SenderId { get; set; }
    [Required]
    public string ReceiverId { get; set; }
}
