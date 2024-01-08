using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023
{
    internal class Day03 : DayBase
    {
        List<ExtractedNumber> _extractedNumbers = new List<ExtractedNumber>();
        List<CachedCharacter> _cachedCharacters = new List<CachedCharacter>();

        public Day03() : base(3)
        {
            // Additional constructor logic for the derived class, if needed
        }

        public override int SolveProblem_1()
        {
            int result = 0;

            ExtractNumbersChars();

            foreach (var number in _extractedNumbers)
            {
                foreach (var character in _cachedCharacters)
                {
                    if (IsNumberTouchingCharacter(number, character))
                    {
                        // Perform actions for numbers touching characters
                        Console.WriteLine($"Number {number.Number} is touching character '{character.Character}'");
                        result += number.Number;
                    }
                }
            }

            return result;
        }

        public override int SolveProblem_2()
        {
            int result = 0;

            ExtractNumbersChars();

            foreach (var character in _cachedCharacters.Where(c => c.Character == '*'))
            {
                List<ExtractedNumber> gears= new List<ExtractedNumber>();
                foreach (var number in _extractedNumbers)
                {
                    if (IsNumberTouchingCharacter(number, character))
                    {
                        gears.Add(number);
                    }
                }

                if (gears.Count >= 2)
                {
                    result += gears.Aggregate(1, (currentProduct, item) => currentProduct * item.Number);
                }
            }

            return result;
        }

        public void ExtractNumbersChars()
        {
            _extractedNumbers = new ();
            _cachedCharacters = new();

            for (int i = 0; i < _textInput.Count; i++)
            {
                string row = _textInput[i];

                for (int j = 0; j < row.Length; j++)
                {
                    char currentChar = row[j];

                    if (char.IsDigit(currentChar))
                    {
                        int startIndex = j;

                        while (j < row.Length && char.IsDigit(row[j]))
                        {
                            j++;
                        }

                        int endIndex = j - 1;
                        int number = int.Parse(row.Substring(startIndex, endIndex - startIndex + 1));
                        _extractedNumbers.Add(new ExtractedNumber
                        {
                            Number = number,
                            RowIndex = i,
                            StartColIndex = startIndex,
                            EndColIndex = endIndex
                        });

                        j--; // Adjusting j to correct index after the while loop
                    }
                    else if (currentChar != '.')
                    {
                        _cachedCharacters.Add(new CachedCharacter
                        {
                            Character = currentChar,
                            RowIndex = i,
                            ColIndex = j
                        });
                    }
                }
            }
        }
        public bool IsNumberTouchingCharacter(ExtractedNumber number, CachedCharacter character)
        {
            int startRow = number.RowIndex;
            int endRow = number.RowIndex;
            int startCol = number.StartColIndex;
            int endCol = number.EndColIndex;

            int charRowIndex = character.RowIndex;
            int charColIndex = character.ColIndex;

            return
            (number.RowIndex == character.RowIndex - 1 && (character.ColIndex >= number.StartColIndex - 1 &&
                                                          character.ColIndex <= number.EndColIndex + 1))
            ||
            (number.RowIndex == character.RowIndex && (character.ColIndex == number.StartColIndex - 1 ||
                                                      character.ColIndex == number.EndColIndex + 1))
            ||
            (number.RowIndex == character.RowIndex + 1 && (character.ColIndex >= number.StartColIndex - 1 &&
                                                          character.ColIndex <= number.EndColIndex + 1));
        }
    }

    public class ExtractedNumber
    {
        public int Number { get; set; }
        public int RowIndex { get; set; }
        public int StartColIndex { get; set; }
        public int EndColIndex { get; set; }
    }

    public class CachedCharacter
    {
        public char Character { get; set; }
        public int RowIndex { get; set; }
        public int ColIndex { get; set; }
    }
}
