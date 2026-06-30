using System;

namespace BookStoreApp.UI
{
    public static class ConsoleHelper
    {
        public static string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Input cannot be empty. Please try again.");
                Console.ResetColor();
            }
        }

        public static int ReadInt(string prompt, int min = 0)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= min)
                    return result;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] Please enter a valid whole number (minimum {min}).");
                Console.ResetColor();
            }
        }

        public static decimal ReadDecimal(string prompt, decimal min = 0)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal result) && result >= min)
                    return result;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] Please enter a valid price (minimum {min}).");
                Console.ResetColor();
            }
        }
        public static string ReadPhoneNumber(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input) &&
                    input.Length == 11 &&
                    input.StartsWith("01") &&
                    input.All(char.IsDigit))
                {
                    return input;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Invalid phone number! It must be 11 digits and start with '01'.");
                Console.ResetColor();
            }
        }
    }
}