using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TheProjector.Application.Persistence;
using TheProjector.Application.Shared;
using TheProjector.Core.Auth;
using TheProjector.Core.Users;
using TheProjector.Core.Shared;

namespace TheProjector.Application.Services.Auth;

public class AuthManager : IAuthManager
{
  private readonly DatabaseContext _dbContext;
  private readonly IHashManager _hashManager;
  private readonly ISmtpManager _smtpManager;
  private readonly IEmailTemplatesManager _emailTemplatesManager;
  private readonly IRandomCodeGenerator _randomCodeGenerator;

  public AuthManager(DatabaseContext dbContext, IUserServices userServices, IHashManager hashManager, ISmtpManager smtpManager, IEmailTemplatesManager emailTemplatesManager, IRandomCodeGenerator randomCodeGenerator)
  {
    _dbContext = dbContext;
    _hashManager = hashManager;
    _smtpManager = smtpManager;
    _emailTemplatesManager = emailTemplatesManager;
    _randomCodeGenerator = randomCodeGenerator;
  }

  public async Task<CommandResult> SignUpAsync(SignupCommand payload)
  {
    var userWithSameEmail = await _dbContext.Users.Where(user => user.Email == payload.Email).SingleOrDefaultAsync();
    if (userWithSameEmail != null) return CommandResult.Error("Email already exists.", "CreateUser.Email");

    var temporaryPassword = _randomCodeGenerator.Generate();
    var person = new Person
    {
      Firstname = payload.Firstname.ToLower(),
      Lastname = payload.Lastname.ToLower(),
      Locale = payload.Locale
    };
    var newUser = new User
    {
      Email = payload.Email,
      RoleId = payload.Role,
      Password = _hashManager.Hash(temporaryPassword),
      StatusId = UserStatus.Pending,
      Person = person,
      PasswordResetToken = "",
    };

    var accountCreationTemplate = _emailTemplatesManager.GetAccountCreationTemplate(temporaryPassword);
    var sendEmailCommand = new SendEmailCommand
    {
      From = "no-reply@theproejector.com",
      To = payload.Email,
      Subject = "Welcome",
      Body = accountCreationTemplate
    };

    _dbContext.Users.Add(newUser);
    await _dbContext.SaveChangesAsync();
    await _smtpManager.SendEmail(sendEmailCommand);

    return CommandResult.Success();
  }

  public async Task<CommandResult> LoginAsync(LoginUserCommand payload)
  {
    var user = await _dbContext.Users
    .Include(user => user.Person)
    .Include(user => user.Role)
    .Include(user => user.Status)
    .Where(user => user.Email == payload.Email).SingleOrDefaultAsync();

    if (user == null) return CommandResult.Error("Incorrect email or password.", string.Empty);
    if (user.Status.Name == "Disabled") return CommandResult.Error("User account has been disabled.", string.Empty);

    if (!_hashManager.Matches(payload.Password, user.Password)) return CommandResult.Error("Incorrect email or password.", string.Empty);

    var claims = new List<Claim>
    {
      new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new Claim(ClaimTypes.Email, user.Email),
      new Claim("Locale", user.Person.Locale),
      new Claim("Status", user.Status.Name),
      new Claim(ClaimTypes.Role, user.Role.Name)
    };

    var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
    var claimsIdentity = new ClaimsIdentity(claims, scheme);

    var authenticationProperties = new AuthenticationProperties
    {
      IsPersistent = true,
      IssuedUtc = DateTimeOffset.UtcNow
    };

    var data = new LoginCommandResultData
    {
      AuthenticationScheme = scheme,
      ClaimsPrincipal = new ClaimsPrincipal(claimsIdentity),
      AuthenticationProperties = authenticationProperties,
    };

    return CommandResult.Success(data);
  }

  public async Task<CommandResult> ActivateAccountAsync(Guid id, ChangePasswordCommand payload)
  {
    if (payload.Password != payload.PasswordConfirmation) return CommandResult.Error("Passwords do not match.", "Payload.PasswordConfirmation");

    var user = await _dbContext.Users.Where(user => user.Id.Equals(id)).SingleOrDefaultAsync();
    if (user == null) return CommandResult.Error("Not Found", "User");

    var newPassword = _hashManager.Hash(payload.Password);
    user.Password = newPassword;
    user.StatusId = UserStatus.Active;

    await _dbContext.SaveChangesAsync();

    return CommandResult.Success();
  }

  public async Task<CommandResult> DisableAccount(Guid id)
  {
    var user = await _dbContext.Users.Where(user => user.Id.Equals(id)).SingleOrDefaultAsync();
    user.StatusId = UserStatus.Disabled;
    await _dbContext.SaveChangesAsync();

    return CommandResult.Success();
  }

  public async Task<CommandResult> RequestResetPasswordAsync(RequestResetPasswordCommand payload)
  {
    var user = await _dbContext.Users.Where(user => user.Email == payload.Email).SingleOrDefaultAsync();
    if (user == null) return CommandResult.Error("Email does not exist.", string.Empty);

    var token = _randomCodeGenerator.Generate(60);
    var resetPasswordTemplate = _emailTemplatesManager.GetResetPasswordTemplate(token);
    var sendEmailCommand = new SendEmailCommand
    {
      From = "no-reply@theproejector.com",
      To = user.Email,
      Subject = "Reset Password Request",
      Body = resetPasswordTemplate
    };

    user.PasswordResetToken = token;
    user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);
    await _dbContext.SaveChangesAsync();
    await _smtpManager.SendEmail(sendEmailCommand);

    return CommandResult.Success();
  }

  public async Task<CommandResult> CheckPasswordResetTokenValidityAsync(string token)
  {
    var user = await _dbContext.Users.Where(user => user.PasswordResetToken == token && user.PasswordResetTokenExpiry > DateTime.UtcNow).SingleOrDefaultAsync();

    if (user == null) return CommandResult.Error("", "");

    return CommandResult.Success();
  }

  public async Task<CommandResult> ChangePasswordAsync(string token, ChangePasswordCommand payload)
  {
    if (payload.Password != payload.PasswordConfirmation) return CommandResult.Error("Passwords do not match.", "Payload.PasswordConfirmation");

    var user = await _dbContext.Users.Where(user => user.PasswordResetToken == token).SingleOrDefaultAsync();

    var newPassword = _hashManager.Hash(payload.Password);

    user.Password = newPassword;
    user.PasswordResetToken = string.Empty;
    user.PasswordResetTokenExpiry = DateTime.MinValue;
    await _dbContext.SaveChangesAsync();

    return CommandResult.Success();
  }
}