using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
  public IActionResult ViewProjects()
  {
    return View();
  }

  [Authorize(Roles = "Manager,Employee")]
  [HttpGet]
  public async Task<IActionResult> ViewProjectsTable(
    [FromQuery] GetProjectsQueryParams queryParams,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
  {
    var currentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    var vm = await new ViewProjectsTableViewModel(_projectServices)
      .Build(currentUserID, queryParams, pageNumber, pageSize);

    return PartialView("_ViewProjectsTable", vm);
  }

  [Authorize(Roles = "Manager,Employee")]
  [HttpGet]
  [Route("/projects/{id:guid}/view")]
  public async Task<IActionResult> ViewProject([FromRoute] Guid id)
  {
    var project = await _projectServices.GetOneAsync(id);
    if (project == null) return View("NotFound");

    var vm = await new ViewProjectViewModel(_projectServices)
      .Build(project);

    return View(vm);
  }

  [Authorize(Roles = "Manager")]
  [HttpPost]
  public async Task<IActionResult> AddProjectMember([FromForm] AddProjectMemberCommand payload)
  {
    var commandResult = await _projectServices.AddProjectMember(payload.ProjectId, payload.UserId);
    var vm = await new ViewProjectMembersControlsViewModel(_projectServices)
        .Build(payload.ProjectId, commandResult);

    return PartialView("_ViewProjectMembersControls", vm);
  }

  [Authorize(Roles = "Manager")]
  [HttpPost]
  public async Task<IActionResult> RemoveProjectMember([FromForm] RemoveProjectMemberCommand payload)
  {
    var commandResult = await _projectServices.RemoveProjectMember(payload.ProjectId, payload.UserId);
    var vm = await new ViewProjectMembersControlsViewModel(_projectServices)
      .Build(payload.ProjectId, commandResult);

    return PartialView("_ViewProjectMembersControls", vm);
  }

  [Authorize(Roles = "Manager,Employee")]
  [HttpGet]
  public async Task<IActionResult> ViewProjectMembersTable(
    [FromQuery] Guid projectId,
    [FromQuery] GetProjectMembersQueryParams queryParams,
    [FromQuery] int pageNumber,
    [FromQuery] int pageSize = 10
  )
  {
    var vm = await new ViewProjectsMembersTableViewModel(_projectServices)
      .Build(projectId, queryParams, pageNumber, pageSize);

    return PartialView("_ViewProjectMembersTable", vm);
  }

  [Authorize(Roles = "Manager")]
  [HttpGet]
  [Route("/projects/create")]
  public async Task<IActionResult> CreateProject()
  {
    var user = await _userServices.GetUserByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
    if (user == null) return View("NotFound");

    return View(new CreateProjectModel { Id = user.Id });
  }

  [Authorize(Roles = "Manager")]
  [HttpPost]
  public async Task<IActionResult> CreatingProject([FromRoute] Guid id, [FromForm] CreateProjectCommand payload)
  {
    if (!ModelState.IsValid) return View(new CreateProjectModel { Id = id, Payload = payload });

    var commandResult = await _projectServices.CreateAsync(id, payload);
    if (!commandResult.IsSuccessful) return this.ViewWithErrors("CreateProject", new CreateProjectModel { Id = id, Payload = payload }, commandResult);

    return RedirectToAction("ViewProjects");
  }

  [Authorize(Roles = "Manager")]
  [HttpGet]
  [Route("/products/{id:guid}/edit")]
  public async Task<IActionResult> EditProject([FromRoute] Guid id)
  {
    var project = await _projectServices.GetOneAsync(id);
    if (project == null) return View("NotFound");

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
    if (!commandResult.IsSuccessful) return this.ViewWithErrors("EditProject", new EditProjectModel { Id = id, Payload = payload }, commandResult);

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