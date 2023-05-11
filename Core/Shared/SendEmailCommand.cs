
namespace TheProjector.Core.Shared;

public class SendEmailCommand
{
  public string From { get; init; }

  public string To { get; init; }

  public string Subject { get; init; }

  public string Body { get; init; }
}