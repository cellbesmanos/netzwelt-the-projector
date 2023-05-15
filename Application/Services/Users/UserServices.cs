using Microsoft.EntityFrameworkCore;
using TheProjector.Application.Persistence;
using TheProjector.Application.Shared;
using TheProjector.Core.Shared;
using TheProjector.Core.Users;

namespace TheProjector.Application.Services.Users;

public class UserServices : IUserServices
{
  private readonly DatabaseContext _dbContext;

  public UserServices(DatabaseContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<IEnumerable<GetRolesQuery>> GetRolesDropdownValuesByUserPermissionAsync(string role)
  {
    var query = _dbContext.Roles.OrderBy(role => role.Name).Select(role => new GetRolesQuery { Id = role.Id, Name = role.Name });

    if (role == "Administrator") return await query.ToListAsync();

    // a non admin, can only add a new employee
    return await query.Where(role => role.Name != "Administrator" && role.Name != "Manager").ToListAsync();
  }

  public async Task<PaginatedList<GetUserQuery>> GetUsers(int pageNumber, int pageSize)
  {
    var users = _dbContext.Users
    .Include(user => user.Person)
    .Include(user => user.Status)
    .Include(user => user.Role)
    .OrderBy(user => user.Person.Firstname + " " + user.Person.Lastname)
    .Select(user => new GetUserQuery
    {
      Id = user.Id,
      Email = user.Email,
      Firstname = user.Person.Firstname,
      Lastname = user.Person.Lastname,
      Role = user.Role.Name,
      Status = user.Status.Name
    });

    return await PaginatedList<GetUserQuery>.Build(users, pageNumber, pageSize);
  }

  public async Task<PaginatedList<GetUserQuery>> GetUsers(GetUsersQueryParams queryParams, int pageNumber, int pageSize)
  {
    var users = _dbContext.Users
    .Include(user => user.Person)
    .Include(user => user.Status)
    .Include(user => user.Role)
    .SearchUser(queryParams.Search)
    .FilterByRole(queryParams.Role)
    .FilterByStatus(queryParams.Status)
    .SortUsers(queryParams.Sort)
    .Select(user => new GetUserQuery
    {
      Id = user.Id,
      Email = user.Email,
      Firstname = user.Person.Firstname,
      Lastname = user.Person.Lastname,
      Role = user.Role.Name,
      Status = user.Status.Name
    });

    return await PaginatedList<GetUserQuery>.Build(users, pageNumber, pageSize);
  }

  public async Task<PaginatedList<GetUserQuery>> GetActiveUsersExcept(GetUsersQueryParams queryParams, int pageNumber, int pageSize, params Guid[] ids)
  {
    var users = _dbContext.Users
    .Include(user => user.Person)
    .Include(user => user.Status)
    .Include(user => user.Role)
    .Where(user => !ids.Contains(user.Id))
    .SearchUser(queryParams.Search)
    .FilterByRole(queryParams.Role)
    .FilterByStatus(queryParams.Status)
    .SortUsers(queryParams.Sort)
    .Select(user => new GetUserQuery
    {
      Id = user.Id,
      Email = user.Email,
      Firstname = user.Person.Firstname,
      Lastname = user.Person.Lastname,
      Role = user.Role.Name,
      Status = user.Status.Name
    });

    return await PaginatedList<GetUserQuery>.Build(users, pageNumber, pageSize);
  }

  public async Task<GetUserQuery?> GetUserByIdAsync(Guid id)
  {
    return await _dbContext.Users
    .Include(user => user.Person)
    .Include(user => user.Role)
    .Include(user => user.Status)
    .Where(user => user.Id == id)
    .Select(user => new GetUserQuery
    {
      Id = user.Id,
      Email = user.Email,
      Firstname = user.Person.Firstname,
      Lastname = user.Person.Lastname,
      Role = user.Role.Name,
      Status = user.Status.Name,
      Locale = user.Person.Locale
    })
    .SingleOrDefaultAsync();
  }

  public async Task<GetUserQuery?> GetUserByEmailAsync(string email)
  {
    return await _dbContext.Users
    .Include(user => user.Person)
    .Include(user => user.Role)
    .Include(user => user.Status)
    .Where(user => user.Email == email)
    .Select(user => new GetUserQuery
    {
      Id = user.Id,
      Email = user.Email,
      Firstname = user.Person.Firstname,
      Lastname = user.Person.Lastname,
      Role = user.Role.Name,
      Status = user.Status.Name,
      Locale = user.Person.Locale
    })
    .SingleOrDefaultAsync();
  }

  public async Task<CommandResult> Update(Guid id, EditProfileCommand payload)
  {
    var userWithSameEmail = await _dbContext.Users.Where(user => user.Email == payload.Email).SingleOrDefaultAsync();
    if (userWithSameEmail != null && !userWithSameEmail.Id.Equals(id)) return CommandResult.Error("Email already exists.", "Payload.Email");

    var user = await _dbContext.Users.Include(user => user.Person).Where(user => user.Id.Equals(id)).SingleOrDefaultAsync();
    if (user == null) return CommandResult.Error("Not Found", "User");

    var logoutUser = user.Email.ToLower() != payload.Email.ToLower();
    user.Person.Firstname = payload.Firstname;
    user.Person.Lastname = payload.Lastname;
    user.Person.Locale = payload.Locale;
    user.Email = payload.Email;
    await _dbContext.SaveChangesAsync();

    var editProfileResult = new EditProfileCommandResult { Logout = logoutUser };

    return CommandResult.Success(editProfileResult);
  }

}