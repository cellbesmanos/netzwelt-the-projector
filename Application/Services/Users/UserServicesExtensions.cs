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

  public static IQueryable<User> FilterUser(this IQueryable<User> users, string filter)
  {
    if (string.IsNullOrEmpty(filter) || filter == "All") return users;

    return users.Where(user => user.Role.Name == filter);
  }
}