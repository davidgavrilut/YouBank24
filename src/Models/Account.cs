using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using RequiredAttribute = Microsoft.Build.Framework.RequiredAttribute;

namespace YouBank24.Models; 
public class Account {
    [Key]
    public Guid AccountId { get; set; }
    [Required]
    public string IBAN { get; set; }
    [Required]
    public string CardNumber { get; set; }
    [Required]
    public DateOnly ExpirationDate { get; set; }
    [Required]
    public string CVV { get; set; }
    [Required]
    public float Balance { get; set; }
    public string ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
}
