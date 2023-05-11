using TheProjector.Core.Projects;

namespace TheProjector.Models.Projects;

public class ProjectMembersModel
{
  public Guid ProjectId { get; set; }

  public bool CurrentUserIsOwner { get; set; }

  public IEnumerable<GetProjectNonMemberQuery> ProjectNonMembers { get; set; }

  public GetProjectMembersQueryParams QueryParams { get; set; }

  public IEnumerable<GetProjectMemberQuery> ProjectMembers { get; set; }
}
