using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AdaSharp
{
    public enum Units
    {
        ADA, Lovelace
    }

    public static class AdaUtils
    {
        public static decimal ToLovelace(decimal amount, Units fromUnits)
        {
            switch (fromUnits)
            {
                case Units.ADA:
                    return amount * 1_000_000;
                case Units.Lovelace:
                    return amount;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fromUnits));
            }
        }

        public static decimal FromLovelace(decimal amount, Units toUnits)
        {
            switch (toUnits)
            {
                case Units.ADA:
                    return amount / 1_000_000;
                case Units.Lovelace:
                    return amount;
                default:
                    throw new ArgumentOutOfRangeException(nameof(toUnits));
            }
        }
        
        /// 1. Pick a long sentence using a wide variety of characters (uppercase, lowercase, whitespace, punctuation, etc).
        ///    Using a computer to randomly generate a passphrase is best, as humans aren't a good source of randomness.
        /// 2. Compute an appropriate hash of this passphrase. 
        ///    You'll need to use an algorithm that yields a 32-byte long string (e.g. SHA256 or BLAKE2b).
        /// 3. Hex-encode the 32-byte hash into a 64-byte sequence of bytes.
        public static string GeneratePassphrase(string passphrase)
        {
            var bytes = Encoding.UTF8.GetBytes(passphrase);
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "");
            }
        }
    }
}
