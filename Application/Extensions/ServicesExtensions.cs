using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Authentication.Cookies;
using TheProjector.Application.Persistence;
using TheProjector.Application.Services.Auth;
using TheProjector.Application.Services.Users;
using TheProjector.Application.Services.Projects;
using TheProjector.Application.Services.Shared;
using TheProjector.Core.Auth;
using TheProjector.Core.Users;
using TheProjector.Core.Projects;
using TheProjector.Core.Shared;

namespace TheProjector.Application.Extensions;

public static class ServicesExtensions
{
  public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
  {
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
    {
      options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
      options.SlidingExpiration = true;
      options.LoginPath = "/auth/login";
      options.AccessDeniedPath = "/forbidden";

      options.Cookie.Name = "connect.sid";
    });

    return services;
  }

  public static IServiceCollection ConfigureDatabaseContext(this IServiceCollection services, IConfiguration config)
  {
    var connectionString = config.GetConnectionString("SqlServerConnection");

    services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

    return services;
  }

  public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
    services.AddScoped<IUrlHelper>(s =>
    {
      var actionContext = s.GetRequiredService<IActionContextAccessor>().ActionContext;
      var factory = s.GetRequiredService<IUrlHelperFactory>();

      return factory.GetUrlHelper(actionContext);
    });

    services.AddScoped<IRandomCodeGenerator, RandomCodeGenerator>();
    services.AddScoped<IHashManager, BCryptHashManager>();
    services.AddScoped<IRazorViewToStringConverter, RazorViewToStringConverter>();

    services.AddTransient<ISmtpManager>(options =>
    {
      var host = config.GetValue<string>("Smtp:Host");
      var port = config.GetValue<int>("Smtp:Port");
      var smtpClient = new SmtpClient
      {
        Host = host,
        Port = port,
        UseDefaultCredentials = false,
        Credentials = null,
        EnableSsl = false
      };

      return new NativeSmtpManager(smtpClient);
    });

    services.AddScoped<IEmailTemplatesManager, EmailTemplatesManager>();
    services.AddScoped<IUserServices, UserServices>();
    services.AddScoped<IAuthManager, AuthManager>();
    services.AddScoped<IProjectServices, ProjectServices>();

    return services;
  }
}