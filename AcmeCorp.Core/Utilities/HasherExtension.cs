using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Core.Utilities
{
    public class HasherExtension
    {
        // Method to generate a salt of specified length
        private static byte[] GenerateSalt(int saltLength)
        {
            byte[] salt = new byte[saltLength];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        public static string GenerateRandomBase64(int length = 32)
        {
            byte[] buffer = new byte[Convert.ToInt32(length)];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(buffer);
            }

            return Convert.ToBase64String(buffer);
        }

        //Return Same Lenght as the Input
        public static string GenerateRandomBase64WithSameLenght(int length = 32)
        {
            int byteLength = (int)Math.Ceiling(length * 0.75); // Calculate byte length for Base64
            byte[] buffer = new byte[byteLength];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(buffer);
            }

            string randomBase64 = Convert.ToBase64String(buffer);

            // Ensure the string has the desired length
            if (randomBase64.Length < length)
            {
                randomBase64 += GenerateRandomBase64(length - randomBase64.Length);
            }
            else if (randomBase64.Length > length)
            {
                randomBase64 = randomBase64.Substring(0, length);
            }

            return randomBase64;
        }

        // Method to hash a password using a salt
        public static string HashPassword(string password, int saltLength = 16, int iterations = 10000, int hashLength = 32)
        {
            byte[] salt = GenerateSalt(saltLength);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                byte[] hash = pbkdf2.GetBytes(hashLength);

                // Combine salt and hash into a single byte array
                byte[] hashBytes = new byte[salt.Length + hash.Length];
                Array.Copy(salt, 0, hashBytes, 0, salt.Length);
                Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);

                // Convert the combined byte array to a string
                string base64Hash = Convert.ToBase64String(hashBytes);

                return base64Hash;
            }
        }

        // Method to check if a password matches the stored hash
        public static bool VerifyPassword(string password, string storedHash, int saltLength = 16, int iterations = 10000, int hashLength = 32)
        {
            // Convert the stored hash string back to a byte array
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Extract the salt from the beginning of the stored hash
            byte[] salt = new byte[saltLength];
            Array.Copy(hashBytes, 0, salt, 0, salt.Length);

            // Compute the hash of the provided password
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                byte[] hash = pbkdf2.GetBytes(hashLength);

                // Compare the computed hash with the stored hash
                for (int i = 0; i < hashLength; i++)
                {
                    if (hashBytes[i + salt.Length] != hash[i])
                    {
                        return false; // Passwords don't match
                    }
                }
            }

            return true; // Passwords match
        }
    }
    
}
