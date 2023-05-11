using Microsoft.AspNetCore.Mvc;
using TheProjector.Core.Auth;

namespace TheProjector.Application.Services.Auth;

public class EmailTemplatesManager : IEmailTemplatesManager
{
  private readonly IUrlHelper _urlHelper;

  public EmailTemplatesManager(IUrlHelper urlHelper)
  {
    _urlHelper = urlHelper;
  }

  public string GetAccountCreationTemplate(string temporaryPassword)
  {
    return $"<h1>Welcome to The Projector!</h1><p>Someone created you an account. Login to activate it.</p><p>Temporary Password: {temporaryPassword}</p>";
  }

  public string GetResetPasswordTemplate(string token)
  {
    var url = _urlHelper.Action(
      "ChangePassword",
      "Auth",
      new RouteValueDictionary { { "token", token } },
      _urlHelper.ActionContext.HttpContext.Request.Scheme,
      "localhost:3000" // hard coded for simplicity
    );

    return $"<h1>We received your request for password reset.</h1><p>To continue please <a href=\"{url}\" target=\"_blank\">click here</a></p>";
  }
}