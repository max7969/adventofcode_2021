using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day11
    {
        public int Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var map = input.Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList()).ToList();
            List<string> mapValues = new List<string>();

            for (int i=0; i<100; i++)
            {
                map = IncreaseAll(map);
                map = Flashes(map);
                mapValues.Add(string.Join("", map.SelectMany(x => x).Select(x => x.ToString()).ToArray()));
            }

            string allValues = string.Join("", mapValues);

            return allValues.Length - allValues.Replace("0", "").Length;
        }

        public int Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var map = input.Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList()).ToList();

            bool allFlashes = false;
            int step = 0;
            while(!allFlashes)
            {
                map = IncreaseAll(map);
                map = Flashes(map);
                step++;
                allFlashes = string.Join("", map.SelectMany(x => x).Select(x => x.ToString()).ToArray()).Replace("0", "").Length == 0;
            }

            return step;
        }

        private List<List<int>> IncreaseAll(List<List<int>> map)
        {
            for (int i=0; i< map.Count; i++)
            {
                for (int j=0; j<map[i].Count; j++)
                {
                    map[i][j] += 1;
                }
            }
            return map;
        }

        private List<List<int>> Flashes(List<List<int>> map)
        {
            while(map.SelectMany(x => x).Any(x => x > 9))
            {
                for (int i = 0; i < map.Count; i++)
                {
                    for (int j = 0; j < map[i].Count; j++)
                    {
                        if (map[i][j] > 9)
                        {
                            foreach(var neighbour in _neighbours)
                            {
                                if (i + neighbour[0] >= 0 && i + neighbour[0] < 10 && j + neighbour[1] >= 0 && j + neighbour[1] < 10)
                                {
                                    if (map[i + neighbour[0]][j + neighbour[1]] != 0)
                                    {
                                        map[i + neighbour[0]][j + neighbour[1]] += 1;
                                    }
                                } 
                            }
                            map[i][j] = 0;
                        }
                    }
                }
            }
            return map;
        }

        private readonly List<int[]> _neighbours = new List<int[]>
        {
            new int[] { -1, -1 },
            new int[] { -1, 0 },
            new int[] { -1, 1 },
            new int[] { 0, -1 },
            new int[] { 0, 1 },
            new int[] { 1, -1 },
            new int[] { 1, 0 },
            new int[] { 1, 1 }
        };
    }
}
