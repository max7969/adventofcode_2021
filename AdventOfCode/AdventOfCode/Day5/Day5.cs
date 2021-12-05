using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day5
    {
        private class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
        public int Compute(string filePath, bool includeDiagonals = false)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            var winds = input.Select(x =>
                x.Split(" -> ").Select(y => new Point() {X = int.Parse(y.Split(",")[0]), Y = int.Parse(y.Split(",")[1]) }).ToArray());

            var map = new Dictionary<string, int>();

            foreach (var wind in winds)
            {

                if (wind[0].X == wind[1].X)
                {
                    for (int i = wind.Select(x => x.Y).Min(); i <= wind.Select(x => x.Y).Max(); i++)
                    {
                        AddPointToMap(map, $"{wind[0].X},{i}");
                        
                    }
                } 
                else if (wind[0].Y == wind[1].Y)
                {
                    for (int i = wind.Select(x => x.X).Min(); i <= wind.Select(x => x.X).Max(); i++)
                    {
                        AddPointToMap(map, $"{i},{wind[0].Y}");
                    }
                }
                else if (includeDiagonals)
                {
                    var sortedWind = wind.OrderBy(x => x.X).ToArray();
                    if (sortedWind[0].Y < sortedWind[1].Y)
                    {
                        do
                        {
                            AddPointToMap(map,$"{sortedWind[0].X++},{sortedWind[0].Y++}");
                        } while (sortedWind[0].X != sortedWind[1].X + 1);
                    }
                    else
                    {
                        do
                        {
                            AddPointToMap(map, $"{sortedWind[0].X++},{sortedWind[0].Y--}");
                        } while (sortedWind[0].X != sortedWind[1].X + 1);
                    }
                }
            }

            return map.Count(x => x.Value > 1);
        }

        private void AddPointToMap(Dictionary<string, int> map, string key)
        {
            if (map.ContainsKey(key))
            {
                map[key] += 1;
            }
            else
            {
                map.Add(key, 1);
            }
        }
    }
}
