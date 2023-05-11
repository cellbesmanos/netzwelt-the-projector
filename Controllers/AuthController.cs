
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TheProjector.Application.Filters;
using TheProjector.Core.Auth;
using TheProjector.Core.Users;
using TheProjector.Models.Auth;

namespace TheProjector.Controllers;

public class AuthController : Controller
{
  private readonly IAuthManager _authManager;
  private readonly IUserServices _userServices;

  public AuthController(IAuthManager authManager, IUserServices userServices)
  {
    _authManager = authManager;
    _userServices = userServices;
  }

  [Authorize(Roles = "Administrator,Manager")]
  [ValidateStatusFilter]
  [HttpGet]
  [Route("/users/create")]
  public async Task<IActionResult> CreateUser()
  {
    var role = User.FindFirst(ClaimTypes.Role).Value;
    return View(new CreateUserModel
    {
      Roles = await _userServices.GetRolesDropdownValuesByUserPermissionAsync(role)
    });
  }

  [Authorize(Roles = "Administrator,Manager")]
  [ValidateStatusFilter]
  [HttpPost]
  public async Task<IActionResult> Signup([FromForm] SignupCommand payload)
  {
    var role = User.FindFirst(ClaimTypes.Role).Value;
    var roles = await _userServices.GetRolesDropdownValuesByUserPermissionAsync(role);

    if (!ModelState.IsValid) return View("CreateUser", new CreateUserModel
    {
      Payload = payload,
      Roles = roles
    });

    var commandResult = await _authManager.SignUpAsync(payload);
    if (!commandResult.IsSuccessful)
    {
      commandResult.Errors.ForEach(err => ModelState.AddModelError(err.FieldName, err.ErrorMessage));

      return View("CreateUser", new CreateUserModel
      {
        Payload = payload,
        Roles = roles
      });
    }

    return View("CreateUser", new CreateUserModel
    {
      Message = "Success! We've sent an email to activate his/her account.",
      Roles = roles
    });
  }

  [AllowAnonymous]
  [HttpGet]
  [Route("/auth/login")]
  public IActionResult Login()
  {
    if (User != null && User.Identity.IsAuthenticated) return RedirectToAction("Index", "Projects");

    return View();
  }

  [AllowAnonymous]
  [HttpPost]
  public async Task<IActionResult> LoginUser([FromForm] LoginUserCommand payload)
  {
    if (!ModelState.IsValid) return View("Login");

    var commandResult = await _authManager.LoginAsync(payload);
    if (!commandResult.IsSuccessful)
    {
      commandResult.Errors.ForEach((err) => ModelState.AddModelError(err.FieldName, err.ErrorMessage));

      return View("Login");
    }

    var data = (LoginCommandResultData)commandResult.Data;
    await HttpContext.SignInAsync(
    data.AuthenticationScheme,
    data.ClaimsPrincipal,
    data.AuthenticationProperties);

    return RedirectToAction("Home", "Users");
  }

  [HttpPost]
  public async Task<IActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToAction("Login");
  }

  [HttpGet]
  [Route("/auth/activate-account")]
  public async Task<IActionResult> ActivateAccount()
  {
    var user = await _userServices.GetUserByIdAsync(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
    if (user == null) return RedirectToAction("NotFound", "Errors");

    if (user.Status.ToLower() != "pending") return RedirectToAction("Index", "Projects");

    return View(new ActivateAccountModel { Id = user.Id });
  }

  [HttpPost]
  public async Task<IActionResult> ActivatingAccount([FromRoute] Guid id, [FromForm] ChangePasswordCommand payload)
  {
    if (!ModelState.IsValid) return View("ActivateAccount", new ActivateAccountModel { Id = id });

    var commandResult = await _authManager.ActivateAccountAsync(id, payload);
    if (!commandResult.IsSuccessful)
    {
      commandResult.Errors.ForEach(err => ModelState.AddModelError(err.FieldName, err.ErrorMessage));

      return View("ActivateAccount", new ActivateAccountModel { Id = id });
    }

    await HttpContext.SignOutAsync();
    return View("Login", new LoginModel { Message = "Your account was successfully activated!" });
  }

  [AllowAnonymous]
  [HttpGet]
  [Route("/auth/reset-password")]
  public IActionResult RequestResetPassword(string email = "")
  {
    return View(new RequestResetPasswordModel { Email = email });
  }

  [AllowAnonymous]
  [HttpPost]
  public async Task<IActionResult> ResettingPassword([FromForm] RequestResetPasswordCommand payload)
  {
    if (!ModelState.IsValid) return View("RequestResetPassword");

    string message = "Reset password instructions sent. Please check your email to continue.";

    var commandResult = await _authManager.RequestResetPasswordAsync(payload);
    if (!commandResult.IsSuccessful) return View("RequestResetPassword", new RequestResetPasswordModel { Message = message });

    return View("RequestResetPassword",
    new RequestResetPasswordModel { Message = message });
  }

  [AllowAnonymous]
  [HttpGet]
  [Route("/auth/change-password")]
  public async Task<IActionResult> ChangePassword([FromQuery] string token)
  {
    var commandResult = await _authManager.CheckPasswordResetTokenValidityAsync(token);
    if (!commandResult.IsSuccessful) return RedirectToAction("NotFound", "Errors");

    return View(new ChangePasswordModel { Token = token });
  }

  [AllowAnonymous]
  [HttpPost]
  public async Task<IActionResult> ChangingPassword([FromRoute] string id, [FromForm] ChangePasswordCommand payload)
  {
    if (!ModelState.IsValid) return View("ChangePassword", new ChangePasswordModel { Token = id });

    var commandResult = await _authManager.ChangePasswordAsync(id, payload);
    if (!commandResult.IsSuccessful)
    {
      commandResult.Errors.ForEach(err => ModelState.AddModelError(err.FieldName, err.ErrorMessage));
      return View("ChangePassword", new ChangePasswordModel { Token = id });
    }

    if (User.Identity.IsAuthenticated) await HttpContext.SignOutAsync();

    return View("Login", new LoginModel { Message = "Reset password was successful." });
  }

  [Authorize(Roles = "Administrator")]
  [ValidateStatusFilter]
  [HttpPost]
  public async Task<IActionResult> DisableUser([FromRoute] Guid id)
  {
    var user = await _userServices.GetUserByIdAsync(id);
    if (user == null) return RedirectToAction("NotFound", "Errors");

    await _authManager.DisableAccount(id);

    return RedirectToAction("ViewUsers", "Users");
  }
}