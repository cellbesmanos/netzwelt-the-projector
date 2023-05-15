
using TheProjector.Application.Shared;
using TheProjector.Core.Projects;

namespace TheProjector.Models.Projects;

public class ViewProjectsViewModel
{
  private readonly IProjectServices _projectServices;

  public PaginatedList<GetProjectQuery> Projects { get; set; }
  public GetProjectsQueryParams QueryParams { get; set; }

  public ViewProjectsViewModel(IProjectServices projectServices)
  {
    _projectServices = projectServices;
  }

  public async Task<ViewProjectsViewModel> Build(Guid currentUserID)
  {
    var queryParams = new GetProjectsQueryParams();
    var projects = await _projectServices.GetUserProjectsAsync(currentUserID, queryParams, 1, 10);

    this.Projects = projects;
    this.QueryParams = queryParams;

    return this;
  }

  public async Task<ViewProjectsViewModel> Build(Guid currentUserID, GetProjectsQueryParams queryParams, int pageNumber, int pageSize)
  {
    var projects = await _projectServices.GetUserProjectsAsync(currentUserID, queryParams, pageNumber, pageSize);

    this.Projects = projects;
    return this;
  }
}