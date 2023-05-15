
using TheProjector.Application.Shared;
using TheProjector.Core.Users;

namespace TheProjector.Models.Users;

public class ViewUsersViewModel
{
  private readonly IUserServices _userServices;

  public PaginatedList<GetUserQuery> Users { get; set; }
  public GetUsersQueryParams QueryParams { get; set; }

  public ViewUsersViewModel(IUserServices userServices)
  {
    _userServices = userServices;
  }

  public async Task<ViewUsersViewModel> Build(Guid currentUserID)
  {
    var queryParams = new GetUsersQueryParams();
    var users = await _userServices.GetActiveUsersExcept(queryParams, 1, 10, currentUserID);

    this.Users = users;
    this.QueryParams = queryParams;

    return this;
  }

  public async Task<ViewUsersViewModel> Build(Guid currentUserId, GetUsersQueryParams queryParams, int pageNumber, int pageSize)
  {
    var users = await _userServices.GetActiveUsersExcept(queryParams, pageNumber, pageSize, currentUserId);

    this.Users = users;
    this.QueryParams = queryParams;

    return this;
  }
}