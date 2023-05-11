using TheProjector.Core.Shared;

namespace TheProjector.Core.Users;

public interface IUserServices
{
  Task<IEnumerable<GetRolesQuery>> GetRolesDropdownValuesByUserPermissionAsync(string role);

  Task<IEnumerable<GetUserQuery>> GetAllUsers();

  Task<IEnumerable<GetUserQuery>> GetAllUsers(GetUsersQueryParams queryParams);

  Task<GetUserQuery> GetUserByIdAsync(Guid id);

  Task<GetUserQuery> GetUserByEmailAsync(string email);

  Task<CommandResult> Update(Guid id, EditProfileCommand payload);
}