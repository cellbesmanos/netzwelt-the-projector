
namespace TheProjector.Core.Auth;

public interface IHashManager
{
  string Hash(string toHash);

  bool Matches(string toCompare, string hash);
}