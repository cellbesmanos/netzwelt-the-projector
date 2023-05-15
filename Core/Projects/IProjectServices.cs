
using TheProjector.Application.Shared;
using TheProjector.Core.Shared;

namespace TheProjector.Core.Projects;

public interface IProjectServices
{
  Task<PaginatedList<GetProjectQuery>> GetUserProjectsAsync(Guid id, GetProjectsQueryParams queryParams, int pageNumber, int pageSize);

  Task<GetProjectQuery> GetOneAsync(Guid id);

  Task<GetProjectQuery> GetOneWithMembersAsync(Guid id);

  Task<GetProjectMemberQuery> GetProjectOwnerAsync(Guid id);

  Task<PaginatedList<GetProjectMemberQuery>> GetProjectMembersAsync(Guid id, GetProjectMembersQueryParams queryParams, int pageNumber, int pageSize);

  Task<PaginatedList<GetProjectMemberQuery>> GetProjectMembersAsync(Guid id, int pageNumber, int pageSize);

  Task<IEnumerable<GetProjectNonMemberQuery>> GetProjectNonMembersAsync(Guid id);

  Task<CommandResult> CreateAsync(Guid id, CreateProjectCommand payload);

  Task<CommandResult> EditAsync(Guid id, EditProjectCommand payload);

  Task<CommandResult> DeleteAsync(Guid id);

  Task<CommandResult> AddProjectMember(Guid projectId, Guid userId);

  Task<CommandResult> RemoveProjectMember(Guid projectId, Guid userId);
}