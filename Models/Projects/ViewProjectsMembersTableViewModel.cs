
using TheProjector.Application.Shared;
using TheProjector.Core.Projects;

namespace TheProjector.Models.Projects;

public class ViewProjectsMembersTableViewModel
{
  public Guid ProjectId { get; set; }
  public GetProjectMemberQuery ProjectOwner { get; set; }
  public PaginatedList<GetProjectMemberQuery> Members { get; set; }
  public GetProjectMembersQueryParams QueryParams { get; set; }

  private readonly IProjectServices _projectServices;

  public ViewProjectsMembersTableViewModel() { }

  public ViewProjectsMembersTableViewModel(IProjectServices projectServices)
  {
    _projectServices = projectServices;
  }

  public async Task<ViewProjectsMembersTableViewModel> Build(Guid projectId, GetProjectMembersQueryParams queryParams, int pageNumber, int pageSize)
  {

    var members = await _projectServices.GetProjectMembersAsync(projectId, queryParams, pageNumber, pageSize);
    var projectOwner = await _projectServices.GetProjectOwnerAsync(projectId);

    this.ProjectId = projectId;
    this.ProjectOwner = projectOwner;
    this.Members = members;
    return this;
  }
}