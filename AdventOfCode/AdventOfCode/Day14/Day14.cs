using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day14
    {
        public long Compute(string filePath, int steps = 10)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            Dictionary<string, string> rules = new Dictionary<string, string>();

            string polymer = input[0];

            Dictionary<string, long> couples = new Dictionary<string, long>();


            foreach (var line in input.Skip(2))
            {
                rules.Add(line.Split(" -> ")[0], line.Split(" -> ")[0].ToCharArray()[0] + line.Split(" -> ")[1] + "+" + line.Split(" -> ")[1] + line.Split(" -> ")[0].ToCharArray()[1]);
                couples.Add(line.Split(" -> ")[0], 0);
            }

            for (int i=0;i<polymer.Length -1; i++)
            {
                couples[polymer.Substring(i, 2)] += 1;
            }

            for (int i=0;i<steps;i++)
            {
                Dictionary<string, long> copyCouples = couples.ToDictionary(x => x.Key, x => (long)0);
                foreach (string key in couples.Keys)
                {
                    copyCouples[rules[key].Split("+")[0]] += couples[key];
                    copyCouples[rules[key].Split("+")[1]] += couples[key];
                }
                couples = copyCouples;
            }

            var results = couples.GroupBy(x => x.Key.Substring(0, 1)).ToDictionary(x => x.Key, x => x.Select(x => x.Value).Sum());
            results[polymer.Last().ToString()] += 1;

            return results.Values.Max() - results.Values.Min();
        }
    }
}
