using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TheProjector.Controllers;

public class ErrorsController : Controller
{
  [HttpGet]
  [Route("/error")]
  public IActionResult UnhandledException()
  {
    var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
    Console.WriteLine(exception?.Error);

    return View();
  }

  [HttpGet]
  [Route("/not-found")]
  public IActionResult NotFound()
  {
    return View();
  }

  [HttpGet]
  [Route("/forbidden")]
  public IActionResult Forbidden()
  {
    return View();
  }
}
