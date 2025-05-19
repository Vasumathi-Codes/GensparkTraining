using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CSharpTasks
{
    internal class Task_09
    {
        
        //9) Write a program that:

        //Has a predefined secret word(e.g., "GAME").

        //Accepts user input as a 4-letter word guess.

        //Compares the guess to the secret word and outputs:

        //X Bulls: number of letters in the correct position.

        //Y Cows: number of correct letters in the wrong position.

        //Continues until the user gets 4 Bulls(i.e., correct guess).

        //Displays the number of attempts.

        //Bull = Correct letter in correct position.

        //Cow = Correct letter in wrong position.

        //Secret Word User Guess  Output Explanation
        //GAME GAME	4 Bulls, 0 Cows Exact match
        //GAME    MAGE    1 Bull, 3 Cows A in correct position, MGE misplaced
        //GAME GUYS	1 Bull, 0 Cows G in correct place, rest wrong
        //GAME AMGE	2 Bulls, 2 Cows A, E right; M, G misplaced
        //NOTE TONE	2 Bulls, 2 Cows O, E right; T, N misplaced

        public void Run()
        {
            const string secretWord = "GAME";
            Console.WriteLine("Clue: It's a 4-letter word!");

            int attempts = 0;

            while (true)
            {
                Console.Write("Enter your guess: ");
                string? guess = Console.ReadLine()?.ToUpper().Trim();
                attempts++;

                if (guess == null || guess.Length != 4)
                {
                    Console.WriteLine("Invalid input. Please enter exactly 4 letters.");
                    continue;
                }

                List<char> bulls = new List<char>(); 
                List<char> cows = new List<char>(); 

                bool[] usedSecret = new bool[4];
                bool[] usedGuess = new bool[4];

                for (int i = 0; i < 4; i++)
                {
                    if (guess[i] == secretWord[i])
                    {
                        bulls.Add(guess[i]);
                        usedSecret[i] = true;
                        usedGuess[i] = true;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (usedGuess[i]) continue;

                    for (int j = 0; j < 4; j++)
                    {
                        if (!usedSecret[j] && guess[i] == secretWord[j])
                        {
                            cows.Add(guess[i]);
                            usedSecret[j] = true;
                            break;
                        }
                    }
                }

                Console.WriteLine($"{guess}    {bulls.Count} Bulls, " +
                  $"{cows.Count} Cows" +
                  $"{(bulls.Count > 0 ? $"  {string.Join("", bulls)} in correct position" : "")}" +
                  $"{(cows.Count > 0 ? $", {string.Join("", cows)} misplaced" : "")}");

                if (bulls.Count == 4)
                {
                    Console.WriteLine($"You found the secret word in {attempts} attempt(s)!");
                    break;
                }
            }

        }
    }
}
