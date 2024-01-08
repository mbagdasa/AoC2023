using AoC2023.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2023
{
    internal class Day01: DayBase
    {
        public Day01() : base(1)
        {
            // Additional constructor logic for the derived class, if needed
        }

        public override int SolveProblem_1()
        {
            int sum = 0;
            foreach (string text in _textInput)
            {
                var numbers = text 
                    .Where(char.IsDigit)
                    .Select(c => int.Parse(c.ToString()))
                    .ToArray();

                if (numbers.Length > 0)
                {
                    int firstNumber = numbers.First();
                    int lastNumber = numbers.Last();
                    string concatenatedNumberString = $"{firstNumber}{lastNumber}";
                    int concatenatedNumber = int.Parse(concatenatedNumberString);

                    sum += concatenatedNumber;
                    Console.WriteLine($"First number: {firstNumber}, Last number: {lastNumber}");
                }
                else
                {
                    Console.WriteLine("No numbers found in the input text.");
                }
            }

            return sum;
        }

        public override int SolveProblem_2()
        {
            // Define a dictionary to map spelled-out numbers to their numerical counterparts
            Dictionary<string, string> spelledOutNumbers = new Dictionary<string, string>
                {
                    { "one", "1" }, { "two", "2" }, { "three", "3" },
                    { "four", "4" }, { "five", "5" }, { "six", "6" },
                    { "seven", "7" }, { "eight", "8" }, { "nine", "9" }
                };

            int sum = 0;
            foreach (string text in _textInput)
            {
                // Regular expression to match digits and spelled-out numbers
                string pattern = @"\d|one|two|three|four|five|six|seven|eight|nine";

                Match first = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
                Match last = Regex.Match(text, pattern, RegexOptions.RightToLeft);
                string firstMatchAsString = first.Success ? first.Value : string.Empty;
                string lastMatchAsString = last.Success ? last.Value : string.Empty;

                var concatenatedNumberString = "";
                if (spelledOutNumbers.ContainsKey(firstMatchAsString))
                {
                    concatenatedNumberString += spelledOutNumbers[firstMatchAsString];
                }
                else
                {
                    concatenatedNumberString += firstMatchAsString;
                }

                if (spelledOutNumbers.ContainsKey(lastMatchAsString))
                {
                    concatenatedNumberString += spelledOutNumbers[lastMatchAsString];
                }
                else
                {
                    concatenatedNumberString += lastMatchAsString;
                }
                sum += int.Parse(concatenatedNumberString);
            }
            return sum;   
        }
    }
}
