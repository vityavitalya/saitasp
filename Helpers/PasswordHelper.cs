using System.Security.Cryptography;
using System.Text;

namespace BookShelf.Helpers;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            return Convert.ToHexString(hashBytes);
        }
    }

    public static bool CheckPassword(string password, string hashFromDb)
    {
        string hash = HashPassword(password);
        return hash == hashFromDb;
    }
}
