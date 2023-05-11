using System.ComponentModel.DataAnnotations;

namespace TheProjector.Core.Auth;

public class LoginUserCommand
{

  [Required(ErrorMessage = "Email is required.")]
  [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
  [MaxLength(256, ErrorMessage = "Email must not be greater than 256 characters.")]
  public string Email { get; init; }

  [Required(ErrorMessage = "Password is required.")]
  public string Password { get; init; }
}