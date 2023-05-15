
using TheProjector.Application.Shared;
using TheProjector.Core.Users;

namespace TheProjector.Models.Users;

public class ViewUsersModel
{
  public PaginatedList<GetUserQuery> Users { get; set; }
}