using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Common.Lib.Security;

public static class Cryptographer
{
    /// <summary>
    /// Hashes a password using secret from configuration
    /// </summary>
    /// <param name="password">Hashed password 32 bytes / 256bit</param>
    /// <param name="secretConfigKey">A key to lookup secret in configuration</param>
    /// <returns></returns>
    public static string HashPassword(string password, string secretConfigKey = "3a8cb5e6-4efe-4a4a-b380-b4013cdb9d12")
    {
        var hashed = "";
        var salt = "4eda2e62-04fa-4915-b333-a12ab001c019";
        var saltBytes = Encoding.ASCII.GetBytes(salt);

        // validating the Password for Null Exception
        if (!string.IsNullOrEmpty(password))
            // Return a 256 bit hash based on SHA256 and 10000 iterations
            hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                saltBytes,
                KeyDerivationPrf.HMACSHA256,
                10000,
                256 / 8)
            );

        return hashed;
    }
}