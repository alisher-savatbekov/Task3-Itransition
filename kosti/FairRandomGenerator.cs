using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace kosti
{
    public class FairRandomGenerator
    {
        private readonly HmacGenerator hmacGenerator;

        public FairRandomGenerator(HmacGenerator hmacGenerator)
        {
            this.hmacGenerator = hmacGenerator;
        }

        public int Generate(int minValue, int maxValue, string hmacValue)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(hmacGenerator.GenerateHmac(hmacValue))))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(hmacValue));
                int seed = BitConverter.ToInt32(hash, 0);
                Random random = new Random(seed);
                return random.Next(minValue, maxValue + 1);
            }
        }
    }

}
