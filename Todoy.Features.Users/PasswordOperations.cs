using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using CryptSharp;

namespace Todoy.Features.Users
{
    public static class PasswordOperations
    {
        public static string GenerateSalt()
        {
            string salt = Crypter.Blowfish.GenerateSalt();

            return salt;
        }

        public static string GenerateHash(string clearTextPassword, string salt)
        {
            if (string.IsNullOrWhiteSpace(salt))
            {
                throw new InvalidOperationException("The salt must be a valid non-empty string");
            }
            else if (string.IsNullOrWhiteSpace(clearTextPassword))
            {
                throw new InvalidOperationException("The password must be a valid non-empty string");
            }
            else
            {
                string saltedPassword = Crypter.Blowfish.Crypt(clearTextPassword, salt);
                return saltedPassword;
            }
        }

    }
}
