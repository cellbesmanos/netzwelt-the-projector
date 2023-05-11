using TheProjector.Core.Users;

namespace TheProjector.Models.Users;

public class ViewUsersModel
{
  public IEnumerable<GetUserQuery> Users { get; set; }

  public GetUsersQueryParams QueryParams { get; set; }
}