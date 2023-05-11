using TheProjector.Core.Auth;
using BC = BCrypt.Net.BCrypt;

namespace TheProjector.Application.Services.Auth;

public class BCryptHashManager : IHashManager
{
  public string Hash(string toHash)
  {
    return BC.HashPassword(toHash);
  }

  public bool Matches(string toCompare, string hash)
  {
    return BC.Verify(toCompare, hash);
  }
}