using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kosti
{

    public class DiceGame
    {
        private readonly HmacGenerator hmacGenerator;
        private readonly FairRandomGenerator randomGenerator;

        public DiceGame(HmacGenerator hmacGenerator, FairRandomGenerator randomGenerator)
        {
            this.hmacGenerator = hmacGenerator;
            this.randomGenerator = randomGenerator;
        }

        public void StartGame(string[][] diceSets)
        {
            bool keepPlaying = true;

            while (keepPlaying)
            {
                Console.WriteLine("\nChoose your dice set:");
                for (int i = 0; i < diceSets.Length; i++)
                {
                    Console.WriteLine($"{i} - {string.Join(", ", diceSets[i])}");
                }

                int userChoice = GetUserChoice(diceSets.Length);
                var userDice = diceSets[userChoice];

                Random random = new Random();
                int computerChoice;
                do
                {
                    computerChoice = random.Next(diceSets.Length);
                } while (computerChoice == userChoice);

                var computerDice = diceSets[computerChoice];

                Console.WriteLine($"\nYour dice set: {string.Join(", ", userDice)}");
                Console.WriteLine($"Computer's dice set: {string.Join(", ", computerDice)}\n");

                string firstMoveHmac = hmacGenerator.GenerateHmac($"first_move_{DateTime.UtcNow.Ticks}");
                int firstMove = randomGenerator.Generate(0, 1, firstMoveHmac);
                Console.WriteLine($"HMAC for the first move: {firstMoveHmac}");
                Console.WriteLine(firstMove == 0 ? "The computer goes first!" : "You go first!\n");

                int userScore = 0, computerScore = 0;

                if (firstMove == 0)
                {
                    Console.WriteLine("Computer's turn:");
                    computerScore = PlayRound(computerDice);
                    Console.WriteLine("Your turn:");
                    userScore = PlayRound(userDice);
                }
                else
                {
                    Console.WriteLine("Your turn:");
                    userScore = PlayRound(userDice);
                    Console.WriteLine("Computer's turn:");
                    computerScore = PlayRound(computerDice);
                }

                DisplayResults(userScore, computerScore);

                Console.WriteLine("\nDo you want to play again? (yes/no)");
                string input = Console.ReadLine()?.ToLower();
                if (input != "yes")
                {
                    keepPlaying = false;
                    Console.WriteLine("Thank you for playing! See you next time!");
                }
            }
        }

        private int GetUserChoice(int diceSetCount)
        {
            while (true)
            {
                Console.WriteLine("Choice range 0-3 // help // exit");
                Console.Write("Your choice: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && choice >= 0 && choice < diceSetCount)
                {
                    return choice;
                }

                if (input == "exit")
                {
                    Console.WriteLine("Exiting the game!");
                    Environment.Exit(0);
                }

                if (input == "help")
                {
                    Console.WriteLine("Game rules: Choose a dice set, roll the dice, and get a result. The one with the highest score wins!");
                }

                Console.WriteLine($"Enter a valid number between 0 and {diceSetCount - 1}.");
            }
        }

        private int PlayRound(string[] dice)
        {
            string hmacValue = hmacGenerator.GenerateHmac($"throw_{DateTime.UtcNow.Ticks}");
            int rollIndex = randomGenerator.Generate(0, dice.Length - 1, hmacValue);
            DiceGameResult result = new DiceGameResult
            {
                RollValue = int.Parse(dice[rollIndex]),
                Hmac = hmacValue
            };
            result.Display();
            return result.RollValue;
        }

        private void DisplayResults(int userScore, int computerScore)
        {
            Console.WriteLine($"\nYour score: {userScore}");
            Console.WriteLine($"Computer's score: {computerScore}");
            if (userScore > computerScore)
            {
                Console.WriteLine("Congratulations! You win!");
            }
            else if (userScore < computerScore)
            {
                Console.WriteLine("The computer wins!");
            }
            else
            {
                Console.WriteLine("It's a draw!");
            }
        }
    }

}
