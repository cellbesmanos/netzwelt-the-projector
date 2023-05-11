
namespace TheProjector.Core.Auth;

public interface IRandomCodeGenerator
{
  string Generate(int length = 20);
}