using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day4
    {
        private class Grid
        {
            public Grid()
            {
                Numbers = new List<List<int>>();
            }

            public List<List<int>> Numbers { get; set; }

            public void Replace(int number)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (Numbers[i][j] == number)
                        {
                            Numbers[i][j] = -1;
                        }
                    }
                }
            }

            public bool IsWinningGrid()
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Numbers[i].Sum() == -5)
                    {
                        return true;
                    }

                    if (Numbers.Select(x => x[i]).Sum() == -5)
                    {
                        return true;
                    }
                }

                return false;
            }

            public int SumOthers()
            {
                return Numbers.SelectMany(x => x).Where(x => x != -1).Sum();
            }
        }

        public int Compute(string filePath, bool playUntilTheEnd)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var draw = input[0].Split(",").Select(int.Parse).ToList();
            List<Grid> grids = new List<Grid>();
            
            foreach (var line in input.Skip(1))
            {
                if (string.IsNullOrEmpty(line))
                {
                    grids.Add(new Grid());
                }
                else
                {
                    grids.Last().Numbers.Add(line.Trim().Replace("  ", " ").Split(" ").Select(int.Parse).ToList());
                }
            }

            foreach (var number in draw)
            {
                grids.ForEach(x => x.Replace(number));

                var winningGrids = grids.Where(x => x.IsWinningGrid()).ToList();
                if (winningGrids.Count > 0 && !playUntilTheEnd)
                {
                    return winningGrids[0].SumOthers() * number;
                }
                
                if (grids.Count == 1 && winningGrids.Count == 1)
                {
                    return winningGrids[0].SumOthers() * number;
                }
                winningGrids.ForEach(x => grids.Remove(x));
            }
            return 0;
        }
    }
}
