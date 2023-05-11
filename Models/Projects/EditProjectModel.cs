
using TheProjector.Core.Projects;

namespace TheProjector.Models.Projects;

public class EditProjectModel
{
  public Guid Id { get; set; }

  public EditProjectCommand Payload { get; set; }
}