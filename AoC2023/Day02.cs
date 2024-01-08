using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2023
{
    internal class Day02 : DayBase
    {
        public Day02() : base(2)
        {
            // Additional constructor logic for the derived class, if needed
        }

        public override int SolveProblem_1()
        {
            int result = 0;
            foreach (string text in _textInput)
            {
                //var textText = "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green";
                List<Bag> allBags = new();

                // Extracting the game number using regex
                Match gameMatch = Regex.Match(text, @"Game (\d+):");
                int gameID = 0;
                if (gameMatch.Success)
                {
                    gameID = int.Parse(gameMatch.Groups[1].Value);
                }

                // Splitting the text after the colon (;)
                string[] splitText = Regex.Split(text.Substring(gameMatch.Index + gameMatch.Length), @";\s*");

                Console.WriteLine($"Game Number: {gameID}");
                foreach (var item in splitText)
                {
                    Bag bag = new();
                    bag.GameID = gameID;
                    string[] pairs = item.Split(',');
                    foreach (string pair in pairs)
                    {
                        string[] parts = pair.Trim().Split(' ');
                        if (parts.Length == 2 && int.TryParse(parts[0], out int count))
                        {
                            Cube cube = new Cube
                            {
                                Color = parts[1],
                                Count = count
                            };
                            bag.Add(cube);
                        }
                        else
                        {
                            Console.WriteLine($"Invalid format for pair: {pair}");
                        }
                    }
                    allBags.Add(bag);
                    Console.WriteLine(item);
                }

                bool invalidGame = allBags.Any(bag => !bag.Possible);

                if (!invalidGame) { result += gameID; }
            }
                

            return result;
        }

        public override int SolveProblem_2()
        {
            int result = 0;
            foreach (string text in _textInput)
            {
                //var textText = "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green";
                List<Bag> allBags = new();

                // Extracting the game number using regex
                Match gameMatch = Regex.Match(text, @"Game (\d+):");
                int gameID = 0;
                if (gameMatch.Success)
                {
                    gameID = int.Parse(gameMatch.Groups[1].Value);
                }

                // Splitting the text after the colon (;)
                string[] splitText = Regex.Split(text.Substring(gameMatch.Index + gameMatch.Length), @";\s*");

                Console.WriteLine($"Game Number: {gameID}");
                foreach (var item in splitText)
                {
                    Bag bag = new();
                    bag.GameID = gameID;
                    string[] pairs = item.Split(',');
                    foreach (string pair in pairs)
                    {
                        string[] parts = pair.Trim().Split(' ');
                        if (parts.Length == 2 && int.TryParse(parts[0], out int count))
                        {
                            Cube cube = new Cube
                            {
                                Color = parts[1],
                                Count = count
                            };
                            bag.Add(cube);
                        }
                        else
                        {
                            Console.WriteLine($"Invalid format for pair: {pair}");
                        }
                    }
                    allBags.Add(bag);
                    Console.WriteLine(item);
                }

                var maxCountsByColor = allBags
                    .SelectMany(bag => bag.Cubes) // Flatten the list of cubes across all bags
                    .GroupBy(cube => cube.Color) // Group cubes by color
                    .Select(group => new
                    {
                        Color = group.Key,
                        MaxCount = group.Max(cube => cube.Count)
                    });

                var maxCountsProduct = maxCountsByColor
                    .Select(item => item.MaxCount) // Extracting the MaxCount values
                    .Aggregate(1, (currentProduct, count) => currentProduct * count);

                result += maxCountsProduct;
                //var bagsWithThreeCubes = allBags.Where(bag => bag.Cubes.Count == 3);

                //var lowestTotalCountsForEachGameID = bagsWithThreeCubes
                //    .GroupBy(bag => bag.GameID)
                //    .Select(group => group.OrderBy(bag => bag.CountTotal).First());
            }


            return result;
        }
    }

    // Helper Classes
    internal class Cube
    {
        public string Color { get; set; }
        public int Count { get; set; }
    }

    internal class Bag
    {
        public int GameID { get; set; }
        public List<Cube> Cubes { get; set; } = new List<Cube>();
        public int CountTotal => Cubes.Sum(b => b.Count);
        public bool Possible { get; set; } = true;

        public void Add(Cube cube)
        {
            if ((cube.Color.Equals("red") && cube.Count > 12) || 
                (cube.Color.Equals("green") && cube.Count > 13) || 
                (cube.Color.Equals("blue") && cube.Count > 14))
            {
                Possible = false;
            }

            Cubes.Add(cube);
        }
    }
}
