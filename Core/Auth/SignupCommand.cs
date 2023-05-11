using System.ComponentModel.DataAnnotations;

namespace TheProjector.Core.Auth;

public class SignupCommand
{
  [Required(ErrorMessage = "Email is required.")]
  [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
  [MaxLength(256, ErrorMessage = "Email must not be greater than 256 characters.")]
  public string Email { get; init; }

  [Required(ErrorMessage = "First name is required.")]
  [MaxLength(64, ErrorMessage = "First name is cannot be greater than 64 characters.")]
  public string Firstname { get; init; }

  [Required(ErrorMessage = "Last name is required.")]
  [MaxLength(64, ErrorMessage = "Last name is cannot be greater than 64 characters.")]
  public string Lastname { get; init; }

  [Required(ErrorMessage = "Role is required.")]
  public Guid Role { get; init; }


  [Required(ErrorMessage = "Locale is required.")]
  public string Locale { get; init; }
}