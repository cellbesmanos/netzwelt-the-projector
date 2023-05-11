using TheProjector.Core.Projects;

namespace TheProjector.Models.Projects;

public class ViewOneProjectModel
{
  public GetProjectMemberQuery Owner { get; set; }

  public GetProjectQuery Project { get; set; }

  public IEnumerable<GetProjectNonMemberQuery> ProjectNonMembers { get; set; }

  public IEnumerable<GetProjectMemberQuery> ProjectMembers { get; set; }
}