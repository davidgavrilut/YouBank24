using FluentNHibernate.Conventions.Inspections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace YouBank24.Models; 
public class Transaction {
    [Key]
    public string TransactionId { get; set; }
    public string? TransactionType { get; set; }
    [Required]
    public float Amount { get; set; }
    [Required]
    public DateTime TransactionTimestamp { get; set; }
    public string TransactionStatus { get; set; }
    [Required]
    public string ReceiverUserId { get; set; }
    public string AccountId { get; set; }
    [ForeignKey("AccountId")]
    [ValidateNever]
    public Account Account { get; set; }
}
