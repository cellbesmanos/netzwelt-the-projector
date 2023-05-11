
using TheProjector.Core.Shared;

namespace TheProjector.Core.Projects;

public interface IProjectServices
{
  Task<IEnumerable<GetProjectQuery>> GetUserProjectsAsync(Guid id);

  Task<IEnumerable<GetProjectQuery>> GetUserProjectsAsync(Guid id, GetProjectsQueryParams queryParams);

  Task<GetProjectQuery> GetOneAsync(Guid id);

  Task<GetProjectQuery> GetOneWithMembersAsync(Guid id);

  Task<GetProjectMemberQuery> GetProjectOwnerAsync(Guid id);

  Task<IEnumerable<GetProjectMemberQuery>> GetProjectMembersAsync(Guid id);

  Task<IEnumerable<GetProjectMemberQuery>> GetProjectMembersAsync(Guid id, GetProjectMembersQueryParams queryParams);

  Task<IEnumerable<GetProjectNonMemberQuery>> GetProjectNonMembersAsync(Guid id);

  Task<CommandResult> CreateAsync(Guid id, CreateProjectCommand payload);

  Task<CommandResult> EditAsync(Guid id, EditProjectCommand payload);

  Task<CommandResult> DeleteAsync(Guid id);

  Task<CommandResult> AddProjectMember(Guid projectId, Guid userId);

  Task<CommandResult> RemoveProjectMember(Guid projectId, Guid userId);
}