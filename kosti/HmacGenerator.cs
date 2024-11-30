using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace kosti
{
    public class HmacGenerator
    {
        private readonly string secretKey;

        public HmacGenerator(string secretKey)
        {
            this.secretKey = secretKey;
        }

        public string GenerateHmac(string message)
        {
            try
            {
                using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
                {
                    byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating HMAC: " + ex.Message);
            }
        }
    }
}