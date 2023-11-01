using AcmeCorp.Domain;
using AcmeCorp.Domain.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Core.Utilities
{
    public class Extension
    {
        public static bool IsStrongPassword(string password)
        {
            // Check if the password is at least 8 characters long
            if (password.Length < 8)
            {
                return false;
            }

            // Check if the password contains at least one number
            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            // Check if the password contains at least one capital letter
            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            // Check if the password contains at least one small letter
            if (!password.Any(char.IsLower))
            {
                return false;
            }

            // Check if the password contains at least one special character
            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                return false;
            }

            return true;
        }

        
    }
}
