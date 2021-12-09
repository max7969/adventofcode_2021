using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day9
    {

        public int Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var map = input.Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList()).ToList();

            var values = new List<int>();
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    var neighbours = new List<int>();
                    if (i - 1 >= 0)
                    {
                        neighbours.Add(map[i-1][j]);
                    }
                    if (i + 1 < map.Count)
                    {
                        neighbours.Add(map[i + 1][j]);
                    }
                    if (j - 1 >= 0)
                    {
                        neighbours.Add(map[i][j - 1]);
                    }
                    if (j + 1 < map[i].Count)
                    {
                        neighbours.Add(map[i][j + 1]);
                    }
                    if (neighbours.All(x => x > map[i][j]))
                    {
                        values.Add(map[i][j]);
                    }
                }
            }

            return values.Select(x => x + 1).Sum();
        }

        public int Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var map = input.Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList()).ToList();

            var sizes = new List<int>();
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    if (map[i][j] != 9 && map[i][j] != -1)
                    {
                        ReplaceNeighbours(map, i, j);
                        sizes.Add(Math.Abs(map.SelectMany(x => x).Where(x => x == -1).Sum()));
                        for (int k=0; k< map.Count; k++)
                        {
                            for (int l=0; l<map[k].Count; l++)
                            {
                                if (map[k][l] == -1)
                                {
                                    map[k][l] = 9;
                                }
                            }
                        }
                    }
                }
            }

            sizes = sizes.OrderByDescending(x => x).ToList();
            return sizes[0] * sizes[1] * sizes[2];
        }

        public void ReplaceNeighbours(List<List<int>> map, int i, int j)
        {
            map[i][j] = -1;
            var borders = new List<int>();
            if (i - 1 >= 0 && map[i - 1][j] != 9 && map[i - 1][j] != -1)
            {
                ReplaceNeighbours(map, i-1, j);
            }
            if (i + 1 < map.Count && map[i + 1][j] != 9 && map[i + 1][j] != -1)
            {
                ReplaceNeighbours(map, i + 1, j);
            }
            if (j - 1 >= 0 && map[i][j - 1] != 9 && map[i][j - 1] != -1)
            {
                ReplaceNeighbours(map, i, j - 1);
            }
            if (j + 1 < map[i].Count && map[i][j + 1] != 9 && map[i][j + 1] != -1)
            {
                ReplaceNeighbours(map, i, j + 1);
            }
        }
    }
}
