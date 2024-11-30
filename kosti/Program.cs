using System;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace kosti
{
    class Program
    {
        public static void Main()
        {
            string secretKey = "ccccddddbbbbb";
            HmacGenerator hmacGenerator = new HmacGenerator(secretKey);
            FairRandomGenerator randomGenerator = new FairRandomGenerator(hmacGenerator);
            DiceGame diceGame = new DiceGame(hmacGenerator, randomGenerator);

            string[][] diceSets = new string[][]
            {
                new string[] { "1", "2", "3", "4", "5", "6" },
                new string[] { "2", "2", "4", "4", "9", "9" },
                new string[] { "1", "1", "6", "6", "8", "8" },
                new string[] { "3", "3", "5", "5", "7", "7" }
            };

            Console.WriteLine("Welcome to the Dice Game!");
            diceGame.StartGame(diceSets);

            Console.WriteLine("\nThank you for playing!");
        }
    }
}