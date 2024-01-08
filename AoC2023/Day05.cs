using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023
{
    internal class Day05 : DayBase
    {
        long _minValue = int.MaxValue;
        List<LongRange> _seeds = new();
        public Day05() : base(5)
        {
            // Additional constructor logic for the derived class, if needed
        }

        public override int SolveProblem_1()
        {
            int result = 0;

            List<long> seeds = new();
            List<Map> mapList = new();
            foreach (string text in _textInput)
            {
                if (text.Contains("seeds:"))
                {
                    seeds = ExtractSeedNumbers(text);
                }
                else if (text.Contains("map"))
                {
                    string[] categories = text.Split(new[] { " map" }, StringSplitOptions.None)[0].Split('-');
                    Map map = new Map
                    {
                        FromCategory = categories[0],
                        ToCategory = categories[2]
                    };
                    mapList.Add(map);
                }
                else if (mapList.Count > 0)
                {
                    List<long> intList = text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                 .Select(long.Parse)
                                 .ToList();
                    mapList.Last().DestinationRangeStartIndexList.Add(intList[0]);
                    mapList.Last().SourceRangeStartIndexList.Add(intList[1]);
                    mapList.Last().RangeLengthList.Add(intList[2]);
                }
            }

            foreach (var seed in seeds)
            {
                SetMappings(seed, "seed", mapList);
            }

            long lowestValue = mapList
            .Where(map => map.ToCategory == "location" && map.MappedNumbers != null && map.MappedNumbers.Any())
            .SelectMany(map => map.MappedNumbers)
            .DefaultIfEmpty(int.MaxValue)
            .Min();

            if (lowestValue <= int.MaxValue && lowestValue >= int.MinValue)
            {
                result = (int)lowestValue;
                Console.WriteLine($"Converted long to int: {result}");
            }
            else
            {
                Console.WriteLine("Cannot convert: Value exceeds the range of int.");
            }
            return result;
        }

        public override int SolveProblem_2()
        {
            int result = 0;

            List<long> seeds = new();
            List<Map> mapList = new();
            foreach (string text in _textInput)
            {
                if (text.Contains("seeds:"))
                {
                    var seedInputList = ExtractSeedNumbers(text);
                    for (int i = 0; i < seedInputList.Count(); i++)
                    {
                        if (!(i == 0 ||  i % 2 == 0))
                        {
                            _seeds.Add(new LongRange(seedInputList[i - 1], seedInputList[i - 1] + seedInputList[i]));
                            //for (int j = 0; j < seedInputList[i]; j++)
                            //{
                            //    seeds.Add(seedInputList[i - 1] + j);
                            //}
                        }                            
                    }
                }
                else if (text.Contains("map"))
                {
                    string[] categories = text.Split(new[] { " map" }, StringSplitOptions.None)[0].Split('-');
                    Map map = new Map
                    {
                        FromCategory = categories[0],
                        ToCategory = categories[2]
                    };
                    mapList.Add(map);
                }
                else if (mapList.Count > 0)
                {
                    List<long> intList = text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                 .Select(long.Parse)
                                 .ToList();
                    mapList.Last().DestinationRangeStartIndexList.Add(intList[0]);
                    mapList.Last().SourceRangeStartIndexList.Add(intList[1]);
                    mapList.Last().RangeLengthList.Add(intList[2]);
                }
            }





            long lowestValue = long.MaxValue;
            foreach (var seed in _seeds)
            {
                SetMappings(seed, "seed", mapList);
                var locValue = mapList.Where(map => map.ToCategory == "location")
                    .SelectMany(map => map.MappedRanges)
                    .Min(range => range.Start);

                if (locValue < lowestValue)
                {
                    lowestValue = locValue;
                }
            }

            if (lowestValue <= int.MaxValue && lowestValue >= int.MinValue)
            {
                result = (int)lowestValue;
                Console.WriteLine($"Converted long to int: {result}");
            }
            else
            {
                Console.WriteLine("Cannot convert: Value exceeds the range of int.");
            }
            return result;
        }

        private List<long> ExtractSeedNumbers(string input)
        {
            List<long> extractedNumbers = new();

            int colonIndex = input.IndexOf(':');
            if (colonIndex != -1 && colonIndex < input.Length - 1)
            {
                string numbersString = input.Substring(colonIndex + 1).Trim();

                extractedNumbers = numbersString
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(str => long.TryParse(str, out long number) ? number : 0) // Parse strings to integers
                    .ToList();
            }
            else
            {
                Console.WriteLine("Colon not found or no numbers after colon.");
            }

            return extractedNumbers;
        }

        private void SetMappings(long value, string category, List<Map> mapList)
        {
            Map mapper = mapList.FirstOrDefault(map => map.FromCategory == category);

            if (mapper is null) { return; }

            // clean previous mapper
            Map mapperPrevious = mapList.FirstOrDefault(map => map.ToCategory == category);
            if (mapperPrevious is not null)
            {
                mapperPrevious.MappedNumbers = null;
            }
            
            long mappedValue = value;
            for (int i = 0; i < mapper.SourceRangeStartIndexList.Count(); i++)
            {
                if (value >= mapper.SourceRangeStartIndexList[i] && value < mapper.SourceRangeStartIndexList[i] + mapper.RangeLengthList[i])
                {
                    mappedValue = value - mapper.SourceRangeStartIndexList[i] + mapper.DestinationRangeStartIndexList[i];  
                }
            }
            mapper.MappedNumbers ??= new List<long>();
            mapper.MappedNumbers.Add(mappedValue);
            SetMappings(mappedValue, mapper.ToCategory, mapList);
        }


        private void SetMappings(LongRange range, string category, List<Map> mapList)
        {
            Map mapper = mapList.FirstOrDefault(map => map.FromCategory == category);

            
            if (mapper is null) { return; }
            if (mapper.ToCategory == "location")
            {
                var minVal = range.Start;

                if (minVal < _minValue)
                {
                    _minValue = minVal;
                }

            }

            if (range is null) { return; }
            for (int i = 0; i < mapper.SourceRangeStartIndexList.Count(); i++)
            {
                LongRange mappingRange = new LongRange(mapper.SourceRangeStartIndexList[i], mapper.SourceRangeStartIndexList[i] + mapper.RangeLengthList[i]-1);
                LongRange mappingRangeDest = new LongRange(mapper.DestinationRangeStartIndexList[i], mapper.DestinationRangeStartIndexList[i] + mapper.RangeLengthList[i]-1);

                // There is an intersection between the ranges
                if (!mappingRange.IntersectsWith(range))
                {
                    mapper.MappedRanges ??= new List<LongRange>();
                    mapper.MappedRanges.Add(range);
                    SetMappings(range, mapper.ToCategory, mapList);
                }
                else
                {
                    var distinctRanges = range.GetDistinctRanges(mappingRange);
                    foreach (var r in distinctRanges)
                    {

                        if (!range.IntersectsWith(r))
                        {
                            mapper.MappedRanges ??= new List<LongRange>();
                            mapper.MappedRanges.Add(r);
                            SetMappings(r, mapper.ToCategory, mapList);
                        }
                        else
                        {
                            mapper.MappedRanges ??= new List<LongRange>();

                            var diff = r.Start - mappingRangeDest.Start;
                            LongRange newRange = new LongRange(r.Start + diff, r.Start + (r.End - r.Start));
                            mapper.MappedRanges.Add(newRange);
                            SetMappings(newRange, mapper.ToCategory, mapList);
                        }

                    }
                }
            }
        }

        // Custom class to represent a LongRange
        public class LongRange
        {
            public long Start { get; }
            public long End { get; }

            public LongRange(long start, long end)
            {
                Start = start;
                End = end;
            }

            public bool IntersectsWith(LongRange otherRange)
            {
                return Start <= otherRange.End && End >= otherRange.Start;
            }

            public IEnumerable<long> GetIntersectionRange(LongRange otherRange)
            {
                long intersectionStart = Math.Max(Start, otherRange.Start);
                long intersectionEnd = Math.Min(End, otherRange.End);

                if (intersectionStart <= intersectionEnd)
                {
                    return Enumerable.Range((int)intersectionStart, (int)(intersectionEnd - intersectionStart + 1)).Select(i => (long)i);
                }

                return Enumerable.Empty<long>();
            }

            public IEnumerable<LongRange> GetDistinctRanges(LongRange otherRange)
            {
                if (!IntersectsWith(otherRange))
                {
                    // No intersection, return both ranges as they are
                    yield return this;
                    yield return otherRange;
                }
                else if (Start <= otherRange.Start && End >= otherRange.End)
                {
                    // Other range is entirely contained within this range
                    if (Start < otherRange.Start)
                    {
                        yield return new LongRange(Start, otherRange.Start - 1);
                    }

                    yield return otherRange;

                    if (End > otherRange.End)
                    {
                        yield return new LongRange(otherRange.End + 1, End);
                    }
                }
                else if (otherRange.Start <= Start && otherRange.End >= End)
                {
                    // This range is entirely contained within the other range
                    if (otherRange.Start < Start)
                    {
                        yield return new LongRange(otherRange.Start, Start - 1);
                    }

                    yield return this;

                    if (otherRange.End > End)
                    {
                        yield return new LongRange(End + 1, otherRange.End);
                    }
                }
                else
                {
                    // Ranges partially overlap
                    long intersectionStart = Math.Max(Start, otherRange.Start);
                    long intersectionEnd = Math.Min(End, otherRange.End);

                    if (Start < intersectionStart)
                    {
                        yield return new LongRange(Start, intersectionStart - 1);
                    }

                    if (End > intersectionEnd)
                    {
                        yield return new LongRange(intersectionEnd + 1, End);
                    }
                }
            }
        }

        public class Map
        {
            public string FromCategory { get; set; }
            public string ToCategory { get; set; }
            public List<long> DestinationRangeStartIndexList { get; set; } = new(); 
            public List<long> SourceRangeStartIndexList { get; set; } = new();
            public List<long> RangeLengthList { get; set; } = new();
            public List<long> MappedNumbers { get; set; } = new();
            public List<LongRange> MappedRanges { get; set; } = new();
            public Map()
            {

            }
        }
    }
}
