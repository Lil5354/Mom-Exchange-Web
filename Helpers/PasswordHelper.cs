// File: Helpers/PasswordHelper.cs
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace B_M.Helpers
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16; // 128 bit 
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10000;

        // Hash password với PBKDF2
        public static string HashPassword(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{Iterations}.{salt}.{key}";
            }
        }

        // Kiểm tra password
        public static bool VerifyPassword(string password, string hash)
        {
            try
            {
                var parts = hash.Split('.');

                if (parts.Length != 3)
                {
                    return false;
                }

                var iterations = Convert.ToInt32(parts[0]);
                var salt = Convert.FromBase64String(parts[1]);
                var key = Convert.FromBase64String(parts[2]);

                using (var algorithm = new Rfc2898DeriveBytes(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256))
                {
                    var keyToCheck = algorithm.GetBytes(KeySize);
                    return keyToCheck.SequenceEqual(key);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

