using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day7
    {
        public int Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var positions = input[0].Split(",").Select(int.Parse).OrderBy(x => x).ToList();
            var median = positions[positions.Count / 2];

            return positions.Sum(x => Math.Abs(x - median));
        }

        public int Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var positions = input[0].Split(",").Select(int.Parse).OrderBy(x => x).ToList();

            var possibilities = new List<int>();
            for (int i = positions.Min(); i <= positions.Max(); i++)
            {
                possibilities.Add(positions.Sum(x => (Math.Abs(x - i) * (Math.Abs(x - i) + 1)) / 2));
            }

            return possibilities.Min();
        }
    }
}
