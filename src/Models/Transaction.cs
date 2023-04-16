using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string ReceiverUserId { get; set; }
    public string ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
    public string AccountId { get; set; }
    [ForeignKey("AccountId")]
    [ValidateNever]
    public Account Account { get; set; }

}
