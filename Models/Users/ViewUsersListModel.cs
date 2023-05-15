using TheProjector.Application.Shared;
using TheProjector.Core.Users;

namespace TheProjector.Models.Users;

public class ViewUsersListModel
{
  public GetUsersQueryParams QueryParams { get; set; }

  public PaginatedList<GetUserQuery> Users { get; set; }
}