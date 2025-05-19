using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CSharpTasks
{
    internal class Task_12
    {
        //12) Write a program that:
        //Takes a message string as input(only lowercase letters, no spaces or symbols).
        //Encrypts it by shifting each character forward by 3 places in the alphabet.
        //Decrypts it back to the original message by shifting backward by 3.
        //Handles wrap-around, e.g., 'z' becomes 'c'.
        //Examples
        //Input:     hello
        //Encrypted: khoor
        //Decrypted: hello
        //-------------
        //Input:     xyz
        //Encrypted: abc
        //Test cases
        //| Input | Shift | Encrypted | Decrypted |
        //| ----- | ----- | --------- | --------- |
        //| hello | 3     | khoor     | hello     |
        //| world | 3     | zruog     | world     |
        //| xyz   | 3     | abc       | xyz       |
        //| apple | 1     | bqqmf     | apple     |

        public void Run() {
            Console.WriteLine("Enter a message string (only lowercase): ");
            string? message = Console.ReadLine();
            while (!string.IsNullOrWhiteSpace(message) && !Regex.IsMatch(message, @"^[a-z]+$"))
            {
                Console.WriteLine("Enter a valid input message");
                message = Console.ReadLine();
            }

            Console.WriteLine($"The message you entered is: {message}");

            string encryptedMessage = Encrypt(message);
            Console.WriteLine( encryptedMessage );

            string descryptedMessage = Decrypt(encryptedMessage);
            Console.WriteLine( descryptedMessage );

        }

        // 'z' - 'a' + 3 ) % 26 ) + 'a'
        // 122 - 97 + 3=(28) ) % 26 ) + 97  => 'c'
        static string Encrypt(string input)
        {
            List<char> chars = new List<char>();
            for(int i=0; i<input.Length; i++)
            {
                chars.Add((char)(((input[i] - 'a' + 3) % 26) + 'a'));
            }

            return string.Join("", chars.ToArray());
        }

        // 'c' - 'a' - 3 + 26)% 26 ) + 'a'
        // 99 - 97 -3 =(-1)+ 26) % 26) + 97 => 'z'
        static string Decrypt(string input)
        {
            List<char> chars = new List<char>();
            for (int i = 0; i < input.Length; i++)
            {
                chars.Add((char)(((input[i] - 'a' - 3 + 26) % 26) + 'a'));
            }

            return string.Join("", chars.ToArray());
        }
    }
}
