
using Microsoft.AspNetCore.Mvc;
using TheProjector.Core.Shared;

namespace TheProjector.Controllers;

public static class ControllerExtensions
{
  public static IActionResult ViewWithErrors(this Controller controller, string viewName, object model, CommandResult commandResult)
  {
    commandResult.Errors.ForEach(err => controller.ModelState.AddModelError(err.FieldName, err.ErrorMessage));

    return controller.View(viewName, model);
  }
}