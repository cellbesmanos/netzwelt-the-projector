using TheProjector.Application.Persistence;

namespace TheProjector.Application.Services.Users;

public static class UserServicesExtensions
{
  public static IQueryable<User> SortUsers(this IQueryable<User> users, string sort)
  {
    if (sort == "asc") return users.OrderBy(user => user.Person.Firstname + " " + user.Person.Lastname);

    return users.OrderByDescending(user => user.Person.Firstname + " " + user.Person.Lastname);
  }

  public static IQueryable<User> SearchUser(this IQueryable<User> users, string search)
  {
    if (string.IsNullOrEmpty(search)) return users;

    return users.Where(user => (user.Person.Firstname.Contains(search) || user.Person.Lastname.Contains(search) || user.Email.Contains(search)));
  }

  public static IQueryable<User> FilterByRole(this IQueryable<User> users, string role)
  {
    if (string.IsNullOrEmpty(role)) return users;

    return users.Where(user => user.Role.Name == role);
  }

  public static IQueryable<User> FilterByStatus(this IQueryable<User> users, string status)
  {
    if (string.IsNullOrEmpty(status)) return users;

    return users.Where(user => user.Status.Name == status);
  }
}