using TheProjector.Application.Persistence;

namespace TheProjector.Application.Services.Projects;
public static class ProjectServicesExtensions
{
  public static IQueryable<Project> SortProjects(this IQueryable<Project> projects, string sort)
  {
    if (sort == "asc") return projects.OrderBy(projects => projects.Name);

    return projects.OrderByDescending(projects => projects.Name);
  }

  public static IQueryable<Project> SearchProject(this IQueryable<Project> projects, string search)
  {
    if (string.IsNullOrEmpty(search)) return projects;

    return projects.Where(project => project.Name.Contains(search) || project.Code.Contains(search));
  }
}