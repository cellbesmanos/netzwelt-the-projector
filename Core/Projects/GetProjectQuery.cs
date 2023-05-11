using TheProjector.Core.Users;

namespace TheProjector.Core.Projects;
public class GetProjectQuery
{
  public Guid Id { get; init; }

  public string Name { get; init; }

  public string Code { get; init; }

  public string Remarks { get; init; }

  public decimal Budget { get; init; }

  public IEnumerable<GetProjectMemberQuery?> Members { get; init; }
}