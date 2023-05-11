using TheProjector.Core.Projects;

namespace TheProjector.Models.Projects;

public class CreateProjectModel
{
  public Guid Id { get; set; }

  public CreateProjectCommand Payload { get; set; }
}