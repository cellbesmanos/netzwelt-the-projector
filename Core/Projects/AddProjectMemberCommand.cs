
namespace TheProjector.Core.Projects;

public class AddProjectMemberCommand
{
  public Guid ProjectId { get; set; }

  public Guid UserId { get; set; }
}