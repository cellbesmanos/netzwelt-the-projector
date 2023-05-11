using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheProjector.Application.Persistence;

public class Person
{
  public Guid Id { get; set; }

  [Required(ErrorMessage = "First name is required.")]
  [MaxLength(64, ErrorMessage = "First name is cannot be greater than 64 characters.")]
  public string Firstname { get; set; }

  [Required(ErrorMessage = "Last name is required.")]
  [MaxLength(64, ErrorMessage = "Last name must not be greater than 64 characters.")]
  public string Lastname { get; set; }

  [Required(ErrorMessage = "Last name is required.")]
  public string Locale { get; set; }

  [ForeignKey(nameof(User))]
  public Guid UserId { get; set; }
  public User User { get; set; } = null!;
}