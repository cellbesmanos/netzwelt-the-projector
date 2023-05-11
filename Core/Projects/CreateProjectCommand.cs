using System.ComponentModel.DataAnnotations;

namespace TheProjector.Core.Projects;

public class CreateProjectCommand
{
  [Required(ErrorMessage = "Name is required.")]
  [MaxLength(64, ErrorMessage = "Name must not be greater than 64 characters.")]
  public string Name { get; init; }

  [Required(ErrorMessage = "Code is required.")]
  [MaxLength(64, ErrorMessage = "Code must not be greater than 64 characters.")]
  [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "Code can only consist of alphanumeric characters and dashes.")]
  public string Code { get; init; }

  [Required(ErrorMessage = "Remarks is required.")]
  [MaxLength(512, ErrorMessage = "Remarks must not be greater than 512 characters.")]
  public string Remarks { get; init; }

  [Required(ErrorMessage = "Budget is required.")]
  [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "Budget must be valid value.")]
  [Range(1, 9999999999999999.9999, ErrorMessage = "Budget must be between 0 and 9, 999, 999, 999, 999, 999.9999.")]
  public decimal Budget { get; init; }
}