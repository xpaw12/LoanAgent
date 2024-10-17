using System.Security.Cryptography;
using System.Text;

namespace LoanAgent.Application.Common.Security.Password;

public static class PasswordManager
{
    public static (string passwordHash, string passwordSalt) CreatePasswordHash(string password)
    {
        var salt = GenerateSaltString();
        var hash = ComputeHash(password + salt);
        return (hash, salt);
    }

    public static bool IsValidPassword(string password, string passwordHash, string salt)
    {
        var hash = ComputeHash(password + salt);
        return hash == passwordHash;
    }

    private static string ComputeHash(string input)
    {
        var hasher = SHA256.Create();
        var passwordBytes = Encoding.Default.GetBytes(input);
        var hash = hasher.ComputeHash(passwordBytes);
        return Convert.ToHexString(hash);
    }

    private static string GenerateSaltString(int size = 16)
    {
        var salt = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return Convert.ToBase64String(salt);
    }
}
