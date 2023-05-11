using TheProjector.Core.Projects;

namespace TheProjector.Models.Projects;

public class AddProjectMemberModel
{
  public Guid Id { get; set; }

  public IEnumerable<GetProjectNonMemberQuery> ProjectNonMembers { get; set; }
}