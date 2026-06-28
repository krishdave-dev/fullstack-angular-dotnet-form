using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DTOs;

 public class UpdateCustomerDto
{
    [Required]
    [MinLength(2)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string City { get; set; } = string.Empty;
}