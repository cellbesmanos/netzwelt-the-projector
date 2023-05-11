using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TheProjector.Application.Shared;
using TheProjector.Application.Filters;
using TheProjector.Models.Projects;
using TheProjector.Core.Users;
using TheProjector.Core.Projects;

namespace TheProjector.Controllers;

[Authorize]
[ValidateStatusFilter]
public class ProjectsController : Controller
{
  private readonly IUserServices _userServices;
  private readonly IProjectServices _projectServices;

  public ProjectsController(IUserServices userServices, IProjectServices projectServices)
  {
    _userServices = userServices;
    _projectServices = projectServices;
  }

  [Authorize(Roles = "Manager,Employee")]
  [HttpGet]
  [Route("/projects")]
  public async Task<IActionResult> ViewProjects()
  {
    var currentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    var projects = await _projectServices.GetUserProjectsAsync(currentUserID);
    var projectsPagedList = PagedList<GetProjectQuery>.ToPagedList(projects, 1, 10);
    var queryParams = new GetProjectsQueryParams { Sort = "asc", Search = "", PageSize = 10 };
    queryParams.TotalPages = projectsPagedList.TotalPages;
    queryParams.PageNumber = projectsPagedList.CurrentPage;

    return View(new ViewProjectsModel { Projects = projectsPagedList, QueryParams = queryParams });
  }

  [Authorize(Roles = "Manager,Employee")]
  [HttpGet]
  public async Task<IActionResult> ViewProjectsList(GetProjectsQueryParams queryParams)
  {
    var currentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    var projects = await _projectServices.GetUserProjectsAsync(currentUserID, queryParams);
    var projectsPagedList = PagedList<GetProjectQuery>.ToPagedList(projects, queryParams.PageNumber, queryParams.PageSize);

    queryParams.TotalPages = projectsPagedList.TotalPages;
    queryParams.PageNumber = projectsPagedList.CurrentPage;

    return PartialView("_ViewProjectsList", new ViewProjectsListModel
    {
      QueryParams = queryParams,
      Projects = projectsPagedList
    });
  }


  [Authorize(Roles = "Manager,Employee")]
  [HttpGet]
  [Route("/projects/{id:guid}/view")]
  public async Task<IActionResult> ViewOneProject([FromRoute] Guid id)
  {
    var project = await _projectServices.GetOneWithMembersAsync(id);
    if (project == null) return View("NotFound");

    var projectOwner = await _projectServices.GetProjectOwnerAsync(id);
    var projectNonMembers = await _projectServices.GetProjectNonMembersAsync(id);

    return View(new ViewOneProjectModel
    {
      Owner = projectOwner,
      Project = project,
      ProjectNonMembers = projectNonMembers
    });
  }

  [Authorize(Roles = "Manager,Employee")]
  [HttpGet]
  public async Task<IActionResult> ViewProjectMembers([FromRoute] Guid id, [FromQuery] GetProjectMembersQueryParams queryParams)
  {
    var currentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    var projectOwner = await _projectServices.GetProjectOwnerAsync(id);
    bool isOwner = currentUserID.Equals(projectOwner.Id);

    var members = (await _projectServices.GetProjectMembersAsync(currentUserID, queryParams)).Where(m => m.Id.Equals(projectOwner.Id));
    var projectMembersPagedList = PagedList<GetProjectMemberQuery>.ToPagedList(members, queryParams.PageNumber, queryParams.PageSize);
    queryParams.TotalPages = projectMembersPagedList.TotalPages;
    queryParams.PageNumber = projectMembersPagedList.CurrentPage;

    return PartialView("_ViewProjectMembers", new ProjectMembersModel
    {
      ProjectId = id,
      CurrentUserIsOwner = isOwner,
      QueryParams = queryParams,
      ProjectMembers = projectMembersPagedList
    });
  }

  [Authorize(Roles = "Manager,Employee")]
  [HttpGet]
  public async Task<IActionResult> ViewProjectMembersList([FromRoute] Guid id, [FromQuery] GetProjectMembersQueryParams queryParams)
  {
    var currentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    var projectOwner = await _projectServices.GetProjectOwnerAsync(id);
    bool isOwner = currentUserID.Equals(projectOwner.Id);

    var members = (await _projectServices.GetProjectMembersAsync(id, queryParams)).Where(m => !m.Id.Equals(projectOwner.Id));
    var projectMembersPagedList = PagedList<GetProjectMemberQuery>.ToPagedList(members, queryParams.PageNumber, queryParams.PageSize);
    queryParams.TotalPages = projectMembersPagedList.TotalPages;
    queryParams.PageNumber = projectMembersPagedList.CurrentPage;

    return PartialView("_ViewProjectMembersList", new ViewProjectMembersListModel
    {
      ProjectId = id,
      IsOwner = isOwner,
      QueryParams = queryParams,
      ProjectMembers = projectMembersPagedList
    });
  }

  [Authorize(Roles = "Manager")]
  [HttpPost]
  public async Task<IActionResult> AddProjectMember([FromBody] AddProjectMemberCommand payload)
  {
    var commandResult = await _projectServices.AddProjectMember(payload.ProjectId, payload.UserId);

    var currentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    var vm = await new ProjectDetailsViewModel(_userServices, _projectServices, currentUserID)
      .CreateView(commandResult, payload);

    return PartialView("_ViewProjectMembers", vm);
  }

  [Authorize(Roles = "Manager")]
  [HttpPost]
  public async Task<IActionResult> RemoveProjectMember([FromBody] RemoveProjectMemberCommand payload)
  {
    var commandResult = await _projectServices.RemoveProjectMember(payload.ProjectId, payload.UserId);

    var currentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    var vm = await new ProjectDetailsViewModel(_userServices, _projectServices, currentUserID)
      .CreateView(commandResult, payload);

    return PartialView("_ViewProjectMembers", vm);
  }

  [Authorize(Roles = "Manager")]
  [HttpGet]
  [Route("/projects/create")]
  public async Task<IActionResult> CreateProject()
  {
    var user = await _userServices.GetUserByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
    if (user == null) return RedirectToAction("NotFound", "Errors");

    return View(new CreateProjectModel { Id = user.Id });
  }

  [Authorize(Roles = "Manager")]
  [HttpPost]
  public async Task<IActionResult> CreatingProject([FromRoute] Guid id, [FromForm] CreateProjectCommand payload)
  {
    if (!ModelState.IsValid) return View(new CreateProjectModel { Id = id, Payload = payload });

    var commandResult = await _projectServices.CreateAsync(id, payload);
    if (!commandResult.IsSuccessful)
    {
      commandResult.Errors.ForEach(err => ModelState.AddModelError(err.FieldName, err.ErrorMessage));

      return View("CreateProject", new CreateProjectModel
      {
        Id = id,
        Payload = payload
      });
    }

    return RedirectToAction("ViewProjects");
  }

  [Authorize(Roles = "Manager")]
  [HttpGet]
  [Route("/products/{id:guid}/edit")]
  public async Task<IActionResult> EditProject([FromRoute] Guid id)
  {
    var project = await _projectServices.GetOneAsync(id);
    if (project == null) return RedirectToAction("NotFound", "Errors");

    var editProjectCommand = new EditProjectCommand
    {
      Name = project.Name,
      Code = project.Code,
      Budget = project.Budget,
      Remarks = project.Remarks
    };

    return View(new EditProjectModel { Id = id, Payload = editProjectCommand });
  }

  [Authorize(Roles = "Manager")]
  [HttpPost]
  public async Task<IActionResult> EditingProject([FromRoute] Guid id, [FromForm] EditProjectCommand payload)
  {
    if (!ModelState.IsValid) return View("EditProject", new EditProjectModel { Id = id, Payload = payload });

    var commandResult = await _projectServices.EditAsync(id, payload);
    if (!commandResult.IsSuccessful)
    {
      commandResult.Errors.ForEach(err => ModelState.AddModelError(err.FieldName, err.ErrorMessage));

      return View("EditProject", new EditProjectModel { Id = id, Payload = payload });
    }

    return RedirectToAction("ViewOneProject", new { Id = id });
  }

  [Authorize(Roles = "Manager")]
  [HttpPost]
  public async Task<IActionResult> DeleteProject([FromRoute] Guid id)
  {
    await _projectServices.DeleteAsync(id);

    return RedirectToAction("ViewProjects");
  }
}