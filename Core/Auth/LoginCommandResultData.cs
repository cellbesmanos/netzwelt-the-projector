using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace TheProjector.Core.Auth;

public class LoginCommandResultData
{
  public string AuthenticationScheme { get; set; }

  public ClaimsPrincipal ClaimsPrincipal { get; set; }

  public AuthenticationProperties AuthenticationProperties { get; set; }
}