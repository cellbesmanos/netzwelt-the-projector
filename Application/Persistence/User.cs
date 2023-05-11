using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheProjector.Application.Persistence;

[Index(nameof(Email), Name = "user_email_index", IsUnique = true)]
public class User
{
  public Guid Id { get; set; }

  [Required(ErrorMessage = "Email is required.")]
  [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
  [MinLength(5, ErrorMessage = "Email must be at least 5 characters long.")]
  [MaxLength(256, ErrorMessage = "Email must not be greater than 256 characters.")]
  public string Email { get; set; }

  [Required(ErrorMessage = "Password is required.")]
  [MaxLength(128, ErrorMessage = "Password is cannot be greater than 128 characters.")]
  public string Password { get; set; }

  public Person? Person { get; set; }

  [ForeignKey(nameof(Role))]
  public Guid RoleId { get; set; }
  public Role Role { get; set; } = null!;

  [ForeignKey(nameof(Status))]
  public Guid StatusId { get; set; }
  public Status Status { get; set; } = null!;

  [MaxLength(64, ErrorMessage = "Password reset token is cannot be greater than 64 characters.")]
  public string PasswordResetToken { get; set; }

  public DateTime PasswordResetTokenExpiry { get; set; }

  public ICollection<Project> Projects { get; }

  public ICollection<ProjectAssignment> ProjectAssignments { get; }
}