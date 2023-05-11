
using TheProjector.Core.Projects;

namespace TheProjector.Models.Projects;

public class ViewProjectMembersListModel
{
  public Guid ProjectId { get; set; }

  public bool IsOwner { get; set; }

  public GetProjectMembersQueryParams QueryParams { get; set; }

  public IEnumerable<GetProjectMemberQuery> ProjectMembers { get; set; }
}