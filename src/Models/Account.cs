using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YouBank24.Models; 
public class Account {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string AccountId { get; set; }
    [Required]
    public string IBAN { get; set; }
    [Required]
    public string CardNumber { get; set; }
    [Required]
    public string ExpirationDate { get; set; }
    [Required]
    public string CVV { get; set; }
    [Required]
    public float Balance { get; set; }
    public string ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
}
