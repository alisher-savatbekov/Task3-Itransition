using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kosti
{

    public class DiceGameResult
    {
        public int RollValue { get; set; }
        public string Hmac { get; set; }

        public void Display()
        {
            Console.WriteLine($"Roll: {RollValue} (HMAC: {Hmac})");
        }
    }

}