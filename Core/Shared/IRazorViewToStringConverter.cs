
using Microsoft.AspNetCore.Mvc;

namespace TheProjector.Core.Shared;

public interface IRazorViewToStringConverter
{
  string Convert(Controller controller, string view, object model = null);
}