
namespace TheProjector.Core.Shared;

public class CommandResult
{
  public List<ValidationMessage> Errors { get; set; }

  public object? Data { get; set; }

  public bool IsSuccessful => Errors == null || Errors.Count() == 0;

  public void AddError(string errorMessage, string fieldName)
  {
    if (Errors == null) Errors = new List<ValidationMessage>();

    Errors.Add(new ValidationMessage { ErrorMessage = errorMessage, FieldName = fieldName });
  }

  public static CommandResult Success(object data = null)
  {
    return new CommandResult { Data = data };
  }

  public static CommandResult Error(string errorMessage, string fieldName)
  {
    var errors = new List<ValidationMessage> { new ValidationMessage { ErrorMessage = errorMessage, FieldName = fieldName } };

    return new CommandResult
    {
      Errors = errors
    };
  }
}