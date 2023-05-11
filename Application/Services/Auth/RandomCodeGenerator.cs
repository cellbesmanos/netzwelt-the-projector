using System.Security.Cryptography;
using System.Text;
using TheProjector.Core.Auth;

namespace TheProjector.Application.Services.Auth;

public class RandomCodeGenerator : IRandomCodeGenerator
{

  public string Generate(int length = 20)
  {
    const string alphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    byte[] randomBytes = new byte[length];
    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(randomBytes);
    }

    StringBuilder sb = new StringBuilder(length);
    foreach (byte b in randomBytes)
    {
      sb.Append(alphanumericChars[b % alphanumericChars.Length]);
    }

    return sb.ToString();
  }
}