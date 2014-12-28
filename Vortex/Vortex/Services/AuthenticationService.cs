using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Vortex.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public string CreateHash(string password, out string salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is null or empty", "password");
            }

            using (var deriveBytes = new Rfc2898DeriveBytes(password, 20)) 
            {
                byte[] saltB = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(24); 

                salt = Convert.ToBase64String(saltB);
                return Convert.ToBase64String(key);
            }
        }

        public bool ValidatePassword(string password, string salt, string correctHash)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is null or empty", "password");
            }

            byte[] saltInBytes = Convert.FromBase64String(salt);
            byte[] correctHashInBytes = Convert.FromBase64String(correctHash);

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltInBytes))
            {
                byte[] passwrodInBytes = deriveBytes.GetBytes(24); // 20-byte key

                if (!passwrodInBytes.SequenceEqual(correctHashInBytes))
                {
                    return false;
                }
            }

            return true;
        }
    }
}