using System.ComponentModel.DataAnnotations;

namespace TheProjector.Application.Persistence;

public class Status
{
  public Guid Id { get; set; }

  [Required(ErrorMessage = "Name is required.")]
  [MaxLength(64, ErrorMessage = "Name must not be greater than 64 characters.")]
  public string Name { get; set; }

  public ICollection<User> Users { get; } = new List<User>();
}