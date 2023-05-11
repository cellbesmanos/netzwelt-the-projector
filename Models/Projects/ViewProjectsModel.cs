using TheProjector.Core.Projects;

namespace TheProjector.Models.Projects;

public class ViewProjectsModel
{
  public IEnumerable<GetProjectQuery> Projects { get; set; }

  public GetProjectsQueryParams QueryParams { get; set; }
}