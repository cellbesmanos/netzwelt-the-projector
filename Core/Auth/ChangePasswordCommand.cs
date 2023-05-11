using System.ComponentModel.DataAnnotations;

namespace TheProjector.Core.Auth;

public class ChangePasswordCommand
{
  [Required(ErrorMessage = "Password is required.")]
  [MinLength(7, ErrorMessage = "Password must be at least 7 characters long.")]
  [MaxLength(20, ErrorMessage = "Password must not be greater than 20 characters.")]
  [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*()_+\-=[\]{}|;':\"",.<>/?]+$", ErrorMessage = "Password must only consist of alphanumeric, special characters and no spaces.")]
  public string Password { get; init; }

  [Required(ErrorMessage = "Password confirmation is required.")]
  public string PasswordConfirmation { get; init; }
}

