
namespace TheProjector.Core.Projects;

public class GetProjectMembersQueryParams
{
  public string Search { get; init; } = string.Empty;

  public string Sort { get; init; } = "asc";

  public string Role { get; init; } = string.Empty;
}