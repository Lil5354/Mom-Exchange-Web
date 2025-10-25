using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace B_M.Helpers
{
    public static class PasswordGenerator
    {
        private static readonly Random Random = new Random();
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NumberChars = "0123456789";
        private const string SpecialChars = "!@#$%^&*";

        public static string GeneratePassword(int length = 12)
        {
            if (length < 8) length = 8;

            var password = new char[length];
            var allChars = LowercaseChars + UppercaseChars + NumberChars + SpecialChars;

            // Ensure at least one character from each category
            password[0] = LowercaseChars[Random.Next(LowercaseChars.Length)];
            password[1] = UppercaseChars[Random.Next(UppercaseChars.Length)];
            password[2] = NumberChars[Random.Next(NumberChars.Length)];
            password[3] = SpecialChars[Random.Next(SpecialChars.Length)];

            // Fill the rest randomly
            for (int i = 4; i < length; i++)
            {
                password[i] = allChars[Random.Next(allChars.Length)];
            }

            // Shuffle the password
            for (int i = 0; i < length; i++)
            {
                int randomIndex = Random.Next(length);
                char temp = password[i];
                password[i] = password[randomIndex];
                password[randomIndex] = temp;
            }

            return new string(password);
        }

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$");
        }
    }
}
