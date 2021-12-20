using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Markup;
using System.Xml;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day20
    {
        public long Compute(string filePath, int steps = 2)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            List<string> code = input[0].ToCharArray().Select(char.ToString).ToList();

            Dictionary<string, string> map = new Dictionary<string, string>();
            for (int i = 2; i < input.Count; i++)
            {
                var splittedLine = input[i].ToCharArray().Select(char.ToString).ToList();
                for (int j = 0; j < splittedLine.Count; j++)
                {
                    map.Add($"{i},{j}", splittedLine[j]);
                }
            }

            Print(map);

            for (int i = 0; i < steps; i++)
            {
                map = AddBorders(map, i, code);
                Print(map);
                var copyMap = map.GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.Single().Value);
                Print(copyMap);
                foreach (var element in map)
                {
                    var selectors = GetSelectors(element.Key);
                    var pos = string.Join("", selectors.Select(x =>
                    {
                        if (map.ContainsKey(x))
                        {
                            return map[x];
                        }
                        else
                        {
                            return i % 2 == 0 ? "." : code[0];
                        }
                    }).ToList());
                    pos = pos.Replace(".", "0").Replace("#", "1");
                    copyMap[element.Key] = code[Convert.ToInt32(pos, 2)];
                }
                map = copyMap;
            }

            Print(map);

            return map.Values.Count(x => x == "#");
        }

        private static void Print(Dictionary<string, string> map)
        {
            var xMin = map.Keys.Select(x => int.Parse(x.Split(",")[0])).Min();
            var xMax = map.Keys.Select(x => int.Parse(x.Split(",")[0])).Max();
            var yMin = map.Keys.Select(x => int.Parse(x.Split(",")[1])).Min();
            var yMax = map.Keys.Select(x => int.Parse(x.Split(",")[1])).Max();

            List<string> print = new List<string>();
            for (int i = xMin; i <= xMax; i++)
            {
                string line = "";
                for (int j = yMin; j <= yMax; j++)
                {
                    if (map.ContainsKey($"{i},{j}"))
                    {
                        line += map[$"{i},{j}"] == "." ? "0" : "1";
                    }
                }
                print.Add(line);
            }
        }

        public Dictionary<string, string> AddBorders(Dictionary<string, string> map, int iteration, List<string> code)
        {
            var xMin = map.Keys.Select(x => int.Parse(x.Split(",")[0])).Min() - 1;
            var xMax = map.Keys.Select(x => int.Parse(x.Split(",")[0])).Max() + 1;
            var yMin = map.Keys.Select(x => int.Parse(x.Split(",")[1])).Min() - 1;
            var yMax = map.Keys.Select(x => int.Parse(x.Split(",")[1])).Max() + 1;

            var border = iteration % 2 == 0 ? "." : code[0];

            for (int i=yMin;i<=yMax;i++)
            {
                map.Add($"{xMin},{i}", border);
                map.Add($"{xMax},{i}", border);
            }
            for (int i=xMin+1;i<xMax;i++)
            {
                map.Add($"{i},{yMin}", border);
                map.Add($"{i},{yMax}", border);
            }
            return map;
        }

        public static List<string> GetSelectors(string input)
        {
            var values = input.Split(",").Select(int.Parse).ToList();
            return new List<string>
            {
                $"{values[0] - 1},{values[1] - 1}",
                $"{values[0] - 1},{values[1]}",
                $"{values[0] - 1},{values[1] + 1}",
                $"{values[0]},{values[1] - 1}",
                $"{values[0]},{values[1]}",
                $"{values[0]},{values[1] + 1}",
                $"{values[0] + 1},{values[1] - 1}",
                $"{values[0] + 1},{values[1]}",
                $"{values[0] + 1},{values[1] + 1}"
            };
        }
    }
}
