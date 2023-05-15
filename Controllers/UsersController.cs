using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using TheProjector.Application.Filters;
using TheProjector.Core.Users;
using TheProjector.Models.Auth;
using TheProjector.Models.Users;

namespace TheProjector.Controllers;

[Authorize]
public class UsersController : Controller
{
  private readonly IUserServices _userServices;

  public UsersController(IUserServices userServices)
  {
    _userServices = userServices;
  }

  [Authorize]
  [ValidateStatusFilter]
  [HttpGet]
  [Route("/")]
  public IActionResult Home()
  {
    if (User.FindFirst(ClaimTypes.Role).Value == "Administrator") return RedirectToAction("ViewUsers", "Users");

    return RedirectToAction("ViewProjects", "Projects");
  }

  [Authorize(Roles = "Administrator")]
  [ValidateStatusFilter]
  [HttpGet]
  [Route("/users")]
  public IActionResult ViewUsers()
  {
    return View();
  }

  [Authorize(Roles = "Administrator")]
  [ValidateStatusFilter]
  [HttpGet]
  public async Task<IActionResult> ViewUsersTable(
    [FromQuery] GetUsersQueryParams queryParams,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
  {
    var vm = await new ViewUsersTableViewModel(_userServices)
      .Build(queryParams, pageNumber, pageSize);

    return PartialView("_ViewUsersTable", vm);
  }

  [Authorize(Roles = "Administrator")]
  [ValidateStatusFilter]
  [HttpGet]
  [Route("/users/{id:guid}/profile")]
  public async Task<IActionResult> ViewUserProfile([FromRoute] Guid id)
  {
    var user = await _userServices.GetUserByIdAsync(id);
    if (user == null) return View("NotFound");

    return View(new ViewUserProfileModel { User = user });
  }

  [HttpGet]
  [ValidateStatusFilter]
  [Route("/profile")]
  public async Task<IActionResult> ViewProfile()
  {
    var user = await _userServices
      .GetUserByIdAsync(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));

    return View(new ViewProfileModel { User = user });
  }

  [HttpGet]
  [ValidateStatusFilter]
  [Route("/profile/{id:guid}/edit")]
  public async Task<IActionResult> EditProfile([FromRoute] Guid id)
  {
    var user = await _userServices.GetUserByIdAsync(id);
    if (user == null) return View("NotFound");

    return View("EditProfile", new EditProfileModel
    {
      Id = id,
      Payload = new EditProfileCommand
      {
        Firstname = user.Firstname,
        Lastname = user.Lastname,
        Email = user.Email,
        Locale = user.Locale
      }
    });
  }

  [HttpPost]
  [ValidateStatusFilter]
  public async Task<IActionResult> EditingProfile([FromRoute] Guid id, [FromForm] EditProfileCommand payload)
  {
    if (!ModelState.IsValid) return View("EditProfile", new EditProfileModel { Id = id, Payload = payload });

    var commandResult = await _userServices.Update(id, payload);
    if (!commandResult.IsSuccessful) return this.ViewWithErrors("EditProfile", new EditProfileModel { Id = id, Payload = payload }, commandResult);

    var editCommandResult = (EditProfileCommandResult)commandResult.Data;
    if (editCommandResult.Logout)
    {
      await HttpContext.SignOutAsync();
      return View("Login", new LoginModel { Message = "Email update was successful." });
    }

    return RedirectToAction("ViewProfile");
  }
}
