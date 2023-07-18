using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel;

namespace YouBank24.Models; 
public class ApplicationUser : IdentityUser {
    [Required]
    [DisplayName("First Name")]
    public string FirstName { get; set; }
    [Required]
    [DisplayName("Last Name")]
    public string LastName { get; set; }
    [Required]
    [DisplayName("Birth Year")]
    public int BirthYear { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    [DisplayName("Postal Code")]
    public string PostalCode { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Country { get; set; }
}
