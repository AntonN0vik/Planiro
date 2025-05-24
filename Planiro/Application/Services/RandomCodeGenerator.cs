using System.Text;

namespace Planiro.Application.Services;

public static class RandomCodeGenerator
{
    //private static readonly Random _random = new Random();
    private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateCode(int length = 8)
    {
        var random = new Random();
        var sb = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            int index = random.Next(Characters.Length);
            sb.Append(Characters[index]);
        }
        return sb.ToString();
    }
}
