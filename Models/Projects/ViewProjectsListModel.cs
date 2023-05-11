using TheProjector.Core.Projects;

namespace TheProjector.Models.Projects;

public class ViewProjectsListModel
{
  public GetProjectsQueryParams QueryParams { get; set; }

  public IEnumerable<GetProjectQuery> Projects { get; set; }
}