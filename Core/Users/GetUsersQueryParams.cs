
using TheProjector.Core.Shared;

namespace TheProjector.Core.Users;

public class GetUsersQueryParams
{
  public string Search { get; init; } = string.Empty;

  public string Sort { get; init; } = "asc";

  public string Role { get; init; } = string.Empty;

  public string Status { get; init; } = "Active";
}