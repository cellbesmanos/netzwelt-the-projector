using TheProjector.Core.Users;

namespace TheProjector.Models.Users;

public class ViewUsersListModel
{
  public Guid UserId { get; set; }

  public GetUsersQueryParams QueryParams { get; set; }

  public IEnumerable<GetUserQuery> Users { get; set; }
}