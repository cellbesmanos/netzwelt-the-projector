
namespace TheProjector.Core.Projects;

public class GetProjectsQueryParams
{
  public string Search { get; init; } = string.Empty;

  public string Sort { get; init; } = "asc";
}