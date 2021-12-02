using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day2
    {
        private class Submarine
        {
            public int Position { get; set; }
            public int Depth { get; set; }
            public int Aim { get; set; }
        }

        public int Compute(string filePath, bool complicatedMode)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var actions = input.Select(x => new { order= x.Split(" ")[0], value= int.Parse(x.Split(" ")[1]) });

            var submarine = new Submarine();
            
            var rules = complicatedMode ? ComplicatedRules : SimpleRules;

            foreach (var action in actions)
            {
                submarine = rules[action.order](action.value, submarine);
            }

            return submarine.Position * submarine.Depth;
        }

        private static readonly Dictionary<string, Func<int, Submarine, Submarine>> SimpleRules =  new Dictionary<string, Func<int, Submarine, Submarine>>()
        {
            {
                "forward",
                (x, sub) =>
                {
                    sub.Position += x;
                    return sub;
                }
            },
            {
                "down",
                (x, sub) =>
                {
                    sub.Depth += x;
                    return sub;
                }
            },
            {
                "up",
                (x, sub) =>
                {
                    sub.Depth -= x;
                    return sub;
                }
            }
        };

        private static readonly Dictionary<string, Func<int, Submarine, Submarine>> ComplicatedRules = new Dictionary<string, Func<int, Submarine, Submarine>>()
        {
            {
                "forward",
                (x, sub) =>
                {
                    sub.Position += x;
                    sub.Depth += sub.Aim * x;
                    return sub;
                }
            },
            {
                "down",
                (x, sub) =>
                {
                    sub.Aim += x;
                    return sub;
                }
            },
            {
                "up",
                (x, sub) =>
                {
                    sub.Aim -= x;
                    return sub;
                }
            }
        };
    }
}
