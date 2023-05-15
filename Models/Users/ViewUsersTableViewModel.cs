
using TheProjector.Application.Shared;
using TheProjector.Core.Users;

namespace TheProjector.Models.Users;

public class ViewUsersTableViewModel
{
  public PaginatedList<GetUserQuery> Users { get; set; }
  public GetUsersQueryParams QueryParams { get; set; }

  private readonly IUserServices _userServices;

  public ViewUsersTableViewModel(IUserServices userServices)
  {
    _userServices = userServices;
  }

  public async Task<ViewUsersTableViewModel> Build(GetUsersQueryParams queryParams, int pageNumber, int pageSize)
  {
    var users = await _userServices.GetUsers(queryParams, pageNumber, pageSize);

    this.Users = users;
    return this;
  }
}