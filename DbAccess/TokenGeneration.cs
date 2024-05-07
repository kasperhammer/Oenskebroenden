using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess
{
    public class TokenGeneration
    {
        private static protected byte[] SecretKey { get; set; }

        public void UpdateSecretKey()
        {
            byte[] keyBytes = new byte[32]; // 256 bits for a strong key

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }

            SecretKey = keyBytes;
        }

        public async Task<byte[]> GetSecretKeyAsync()
        {

            if (SecretKey != null)
            {
                return SecretKey;
            }
            else
            {
                UpdateSecretKey();
                if (SecretKey != null)
                {
                    return SecretKey;
                }
            }
            return null;
        }
    }
}
