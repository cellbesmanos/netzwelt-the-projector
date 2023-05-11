
namespace TheProjector.Core.Projects;

public class RemoveProjectMemberCommand
{
  public Guid ProjectId { get; set; }

  public Guid UserId { get; set; }
}