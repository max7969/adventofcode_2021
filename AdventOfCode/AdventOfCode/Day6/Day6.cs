using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day6
    {
        private class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
        public long Compute(string filePath, int days = 80)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            var fishes = input.First().Split(",").Select(long.Parse).ToList();
            var map = new Dictionary<long, long>();
            for (int i = 0; i <= 8; i++)
            {
                map.Add(i, fishes.Count(x => x == i));
            }

            for (int i = 0; i < days; i++)
            {
                var mapCopy = new Dictionary<long, long>(map);
                for (int j = 8; j >= 0; j--)
                {
                    if (j == 0)
                    {
                        mapCopy[8] = map[j];
                        mapCopy[6] += map[j];
                    }
                    else
                    {
                        mapCopy[j - 1] = map[j];
                    }
                }

                map = mapCopy;
            }
            return map.Sum(x => x.Value);
        }
    }
}
