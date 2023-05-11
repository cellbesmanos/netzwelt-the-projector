using Microsoft.EntityFrameworkCore;
using TheProjector.Application.Persistence;
using TheProjector.Application.Services.Users;
using TheProjector.Core.Shared;
using TheProjector.Core.Projects;

namespace TheProjector.Application.Services.Projects;

public class ProjectServices : IProjectServices
{
  private readonly DatabaseContext _dbContext;
  public ProjectServices(DatabaseContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<GetProjectQuery> GetOneAsync(Guid id)
  {
    return await _dbContext.Projects
    .Where(project => project.Id.Equals(id))
    .Select(project => new GetProjectQuery
    {
      Id = project.Id,
      Name = project.Name,
      Code = project.Code,
      Remarks = project.Remarks,
      Budget = project.Budget
    }).SingleOrDefaultAsync();
  }

  public async Task<GetProjectQuery> GetOneWithMembersAsync(Guid id)
  {
    return await _dbContext.Projects
    .Where(project => project.Id.Equals(id))
    .Include(project => project.Users)
      .ThenInclude(user => user.Person)
    .Select(project =>
    new GetProjectQuery
    {
      Id = project.Id,
      Name = project.Name,
      Code = project.Code,
      Budget = project.Budget,
      Remarks = project.Remarks,
      Members = project.Users
      .Select(user =>
      new GetProjectMemberQuery { Id = user.Id, Firstname = user.Person.Firstname, Lastname = user.Person.Lastname }).ToList()
    }).SingleOrDefaultAsync();
  }

  public async Task<IEnumerable<GetProjectQuery>> GetUserProjectsAsync(Guid id)
  {
    return await _dbContext.Projects
    .Join(_dbContext.ProjectAssignments, (p) => p.Id, (pa) => pa.ProjectId, (p, pa) => new
    {
      Id = p.Id,
      Name = p.Name,
      Code = p.Code,
      Remarks = p.Remarks,
      Budget = p.Budget,
      UserId = pa.UserId
    })
    .Where(project => project.UserId.Equals(id))
    .Select(project => new GetProjectQuery
    {
      Id = project.Id,
      Name = project.Name,
      Code = project.Code,
      Remarks = project.Remarks,
      Budget = project.Budget
    })
    .ToListAsync();
  }

  public async Task<IEnumerable<GetProjectQuery>> GetUserProjectsAsync(Guid id, GetProjectsQueryParams getProjectsQueryParams)
  {
    return await _dbContext.Projects
    .SearchProject(getProjectsQueryParams.Search)
    .SortProjects(getProjectsQueryParams.Sort)
    .Join(_dbContext.ProjectAssignments, (p) => p.Id, (pa) => pa.ProjectId, (p, pa) => new
    {
      Id = p.Id,
      Name = p.Name,
      Code = p.Code,
      Remarks = p.Remarks,
      Budget = p.Budget,
      UserId = pa.UserId
    })
    .Where(project => project.UserId.Equals(id))
    .Select(project => new GetProjectQuery
    {
      Id = project.Id,
      Name = project.Name,
      Code = project.Code,
      Remarks = project.Remarks,
      Budget = project.Budget
    })
    .ToListAsync();
  }

  public async Task<GetProjectMemberQuery> GetProjectOwnerAsync(Guid id)
  {
    return await _dbContext.Users
    .Include(user => user.Person)
    .Join(_dbContext.ProjectAssignments, (user) => user.Id, (pa) => pa.UserId,
    (user, pa) => new
    {
      Id = user.Id,
      Firstname = user.Person.Firstname,
      Lastname = user.Person.Lastname,
      IsOwner = pa.IsOwner,
      ProjectId = pa.ProjectId
    })
    .Where(user => user.IsOwner == true && user.ProjectId.Equals(id))
    .Select(user => new GetProjectMemberQuery
    {
      Id = user.Id,
      Firstname = user.Firstname,
      Lastname = user.Lastname
    })
    .SingleOrDefaultAsync();
  }

  public async Task<IEnumerable<GetProjectMemberQuery>> GetProjectMembersAsync(Guid id)
  {
    return await _dbContext.Users
    .Include(user => user.Person)
    .Include(user => user.Status)
    .Include(user => user.Role)
    .Where(user => user.Role.Name == "Employee" && user.Status.Name == "Active")
    .Where((user) => _dbContext.ProjectAssignments
      .Where(pa => pa.ProjectId.Equals(id))
      .Select(pa => pa.UserId)
      .Contains(user.Id))
    .Select((user) => new GetProjectMemberQuery { Id = user.Id, Firstname = user.Person.Firstname, Lastname = user.Person.Lastname })
    .ToListAsync();
  }

  public async Task<IEnumerable<GetProjectMemberQuery>> GetProjectMembersAsync(Guid id, GetProjectMembersQueryParams queryParams)
  {
    return await _dbContext.Users
    .Include(user => user.Person)
    .Include(user => user.Status)
    .Include(user => user.Role)
    .SearchUser(queryParams.Search)
    .SortUsers(queryParams.Sort)
    .Where(user => user.Role.Name == "Employee" && user.Status.Name == "Active")
    .Where((user) => _dbContext.ProjectAssignments
      .Where(pa => pa.ProjectId.Equals(id))
      .Select(pa => pa.UserId)
      .Contains(user.Id))
    .Select((user) => new GetProjectMemberQuery { Id = user.Id, Firstname = user.Person.Firstname, Lastname = user.Person.Lastname })
    .ToListAsync();
  }

  public async Task<IEnumerable<GetProjectNonMemberQuery>> GetProjectNonMembersAsync(Guid id)
  {
    return await _dbContext.Users
    .Include(user => user.Person)
    .Include(user => user.Status)
    .Include(user => user.Role)
    .Where(user => user.Role.Name == "Employee" && user.Status.Name == "Active")
    .Where((user) => !_dbContext.ProjectAssignments
      .Where(pa => pa.ProjectId.Equals(id))
      .Select(pa => pa.UserId)
      .Contains(user.Id))
    .Select((user) => new GetProjectNonMemberQuery { Id = user.Id, Firstname = user.Person.Firstname, Lastname = user.Person.Lastname })
    .ToListAsync();
  }


  public async Task<CommandResult> CreateAsync(Guid id, CreateProjectCommand payload)
  {
    var projectWithSameCode = await _dbContext.Projects.Where(project => project.Code == payload.Code).SingleOrDefaultAsync();
    if (projectWithSameCode != null) return CommandResult.Error("Code already exists.", "Payload.Code");

    var project = new Project
    {
      Name = payload.Name,
      Code = payload.Code.ToUpper(),
      Remarks = payload.Remarks,
      Budget = payload.Budget,
    };

    _dbContext.Projects.Add(project);
    await _dbContext.SaveChangesAsync();

    var projectAssignment = new ProjectAssignment
    {
      UserId = id,
      ProjectId = project.Id,
      IsOwner = true,
    };

    _dbContext.ProjectAssignments.Add(projectAssignment);
    await _dbContext.SaveChangesAsync();

    return CommandResult.Success();
  }

  public async Task<CommandResult> EditAsync(Guid id, EditProjectCommand payload)
  {
    var project = await _dbContext.Projects.Where(project => project.Id.Equals(id)).SingleOrDefaultAsync();
    if (project == null) return CommandResult.Error("Not Found", "Project");

    var projectWithSameCode = await _dbContext.Projects.Where(project => project.Code == payload.Code).SingleOrDefaultAsync();
    if (projectWithSameCode != null && !projectWithSameCode.Equals(project)) return CommandResult.Error("Code already exists.", "Payload.Code");

    project.Name = payload.Name;
    project.Code = payload.Code.ToUpper();
    project.Budget = payload.Budget;
    project.Remarks = payload.Remarks;
    await _dbContext.SaveChangesAsync();

    return CommandResult.Success();
  }

  public async Task<CommandResult> DeleteAsync(Guid id)
  {
    var project = await _dbContext.Projects.Where(project => project.Id.Equals(id)).SingleOrDefaultAsync();
    if (project == null) return CommandResult.Error("Not Found", "Project");

    _dbContext.Projects.Remove(project);
    await _dbContext.SaveChangesAsync();

    return CommandResult.Success();
  }

  public async Task<CommandResult> AddProjectMember(Guid projectId, Guid userId)
  {
    var member = await _dbContext.Users.Where(user => user.Id.Equals(userId)).SingleOrDefaultAsync();
    if (member == null) return CommandResult.Error("NotFound", "User");

    var project = await _dbContext.Projects.Where(project => project.Id.Equals(projectId)).SingleOrDefaultAsync();
    if (project == null) return CommandResult.Error("NotFound", "Project");

    _dbContext.ProjectAssignments.Add(new ProjectAssignment { ProjectId = projectId, UserId = userId });
    await _dbContext.SaveChangesAsync();

    return CommandResult.Success();
  }

  public async Task<CommandResult> RemoveProjectMember(Guid projectId, Guid userId)
  {
    var member = await _dbContext.Users.Where(user => user.Id.Equals(userId)).SingleOrDefaultAsync();
    if (member == null) return CommandResult.Error("NotFound", "User");

    var project = await _dbContext.Projects.Where(project => project.Id.Equals(projectId)).SingleOrDefaultAsync();
    if (project == null) return CommandResult.Error("NotFound", "Project");

    var pa = await _dbContext.ProjectAssignments.Where(pa => pa.Project.Id.Equals(projectId) && pa.UserId.Equals(userId)).SingleOrDefaultAsync();

    _dbContext.ProjectAssignments.Remove(pa);
    await _dbContext.SaveChangesAsync();

    return CommandResult.Success();
  }
}