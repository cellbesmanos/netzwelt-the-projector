
using TheProjector.Application.Shared;
using TheProjector.Core.Projects;

namespace TheProjector.Models.Projects;

public class ViewProjectsTableViewModel
{
  public PaginatedList<GetProjectQuery> Projects { get; set; }
  public GetProjectsQueryParams QueryParams { get; set; }

  private readonly IProjectServices _projectServices;

  public ViewProjectsTableViewModel(IProjectServices projectServices)
  {
    _projectServices = projectServices;
  }

  public async Task<ViewProjectsTableViewModel> Build(Guid currentUserId, GetProjectsQueryParams queryParams, int pageNumber, int pageSize)
  {
    var projects = await _projectServices.GetUserProjectsAsync(currentUserId, queryParams, pageNumber, pageSize);

    this.Projects = projects;
    return this;
  }
}