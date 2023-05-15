
using TheProjector.Core.Shared;
using TheProjector.Application.Shared;

namespace TheProjector.Core.Users;

public interface IUserServices
{
  Task<IEnumerable<GetRolesQuery>> GetRolesDropdownValuesByUserPermissionAsync(string role);

  Task<PaginatedList<GetUserQuery>> GetUsers(int pageNumber, int pageSize);

  Task<PaginatedList<GetUserQuery>> GetUsers(GetUsersQueryParams queryParams, int pageNumber, int pageSize);

  Task<PaginatedList<GetUserQuery>> GetActiveUsersExcept(GetUsersQueryParams queryParams, int pageNumber, int pageSize, params Guid[] ids);

  Task<GetUserQuery?> GetUserByIdAsync(Guid id);

  Task<GetUserQuery?> GetUserByEmailAsync(string email);

  Task<CommandResult> Update(Guid id, EditProfileCommand payload);
}