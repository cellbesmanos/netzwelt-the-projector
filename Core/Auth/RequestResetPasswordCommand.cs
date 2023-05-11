using System.ComponentModel.DataAnnotations;

namespace TheProjector.Core.Auth;

public class RequestResetPasswordCommand
{
  [Required(ErrorMessage = "Email is required.")]
  [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
  public string Email { get; init; }
}