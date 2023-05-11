using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheProjector.Application.Persistence;

[Index(nameof(Code), Name = "project_code_index", IsUnique = true)]

public class Project
{
  public Guid Id { get; set; }

  [Required(ErrorMessage = "Name is required.")]
  [MaxLength(64, ErrorMessage = "Name must not be greater than 64 characters.")]
  public string Name { get; set; }

  [Required(ErrorMessage = "Code is required.")]
  [MaxLength(64, ErrorMessage = "Code must not be greater than 64 characters.")]
  public string Code { get; set; }

  [Required(ErrorMessage = "Remarks is required.")]
  [Column(TypeName = "nvarchar(MAX)")]
  [MaxLength(512, ErrorMessage = "Remarks must not be greater than 512 characters.")]
  public string Remarks { get; set; }

  [Required(ErrorMessage = "Name is required.")]
  [Column(TypeName = "decimal(18, 4)")]
  public decimal Budget { get; set; }

  public ICollection<User> Users { get; }

  public ICollection<ProjectAssignment> ProjectAssignments { get; }
}