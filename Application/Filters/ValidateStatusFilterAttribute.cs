
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TheProjector.Core.Users;

namespace TheProjector.Application.Filters;

public class ValidateStatusFilterAttribute : Attribute, IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    if (!context.HttpContext.User.Identity.IsAuthenticated)
    {
      context.Result = new ViewResult { ViewName = "Login" };
      return;
    };

    var userServices = context.HttpContext.RequestServices.GetService<IUserServices>();
    var userId = Guid.Parse(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
    var user = await userServices.GetUserByIdAsync(userId);

    if (user.Status.ToLower() == "pending")
    {
      context.Result = new RedirectToActionResult("ActivateAccount", "Auth", routeValues: null);
      return;
    }

    if (user.Status.ToLower() == "disabled")
    {
      context.Result = new ViewResult { ViewName = "Login" };
      return;
    }

    await next();
  }
}