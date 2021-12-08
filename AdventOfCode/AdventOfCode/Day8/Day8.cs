using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day8
    {
        private class Entry
        {
            public List<string> Signals { get; set; }
            public List<String> Output { get; set; }
        } 
        public int Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var entries = input.Select(x => new Entry
            {
                Signals = x.Split(" | ")[0].Split(" ").Select(y => string.Concat(y.OrderBy(z => z).ToArray())).ToList(),
                Output = x.Split(" | ")[1].Split(" ").Select(y => string.Concat(y.OrderBy(z => z).ToArray())).ToList()
            });


            return entries.SelectMany(x => x.Output).Count(IsSimpleDigit);
        }

        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var entries = input.Select(x => new Entry
            {
                Signals = x.Split(" | ")[0].Split(" ").ToList(),
                Output = x.Split(" | ")[1].Split(" ").ToList()
            });

            long sum = 0;
            foreach (var entry in entries)
            {
                var fixes = new List<string> { entry.Signals.Single(x => x.Length == 2), entry.Signals.Single(x => x.Length == 4), entry.Signals.Single(x => x.Length == 3) };
                var segments = _functions.Select(x => x(entry.Signals, fixes)).ToList();

                string numeric = "";
                foreach (var value in entry.Output)
                {
                   string result = value;
                   for (int i = 0; i < segments.Count; i++)
                   {
                        result = result.Replace(segments[i], i.ToString());
                   }
                   numeric += _digits[string.Concat(result.OrderBy(x => x).ToArray())];
                }

                sum += long.Parse(numeric);
            }

            return sum;
        }

        private bool IsSimpleDigit(string value)
        {
            List<int> simpleSize = new List<int> { 2, 3, 4, 7 };
            return simpleSize.Contains(value.Length);
        }

        private readonly List<Func<List<string>, List<String>, string>> _functions = new List<Func<List<string>, List<string>, string>>
        {
            (possibilities, fixes) => fixes[2].ToCharArray().First(x => !fixes[0].ToCharArray().Contains(x)).ToString(),
            (possibilities, fixes) => possibilities.SelectMany(x => x.ToCharArray()).GroupBy(x => x).First(x =>
                x.Count() == 6 && !fixes[0].ToCharArray().Contains(x.Key) && !fixes[2].ToCharArray().Contains(x.Key)).Key.ToString(),
            (possibilities, fixes) => possibilities.SelectMany(x => x.ToCharArray()).GroupBy(x => x).First(x =>
                x.Count() == 8 && fixes[0].ToCharArray().Contains(x.Key) && fixes[2].ToCharArray().Contains(x.Key)).Key.ToString(),
            (possibilities, fixes) => possibilities.SelectMany(x => x.ToCharArray()).GroupBy(x => x).First(x =>
                x.Count() == 7 && !fixes[0].ToCharArray().Contains(x.Key) && !fixes[2].ToCharArray().Contains(x.Key) && fixes[1].ToCharArray().Contains(x.Key)).Key.ToString(),
            (possibilities, fixes) => possibilities.SelectMany(x => x.ToCharArray()).GroupBy(x => x).First(x =>
                x.Count() == 4 && !fixes[0].ToCharArray().Contains(x.Key) && !fixes[2].ToCharArray().Contains(x.Key)).Key.ToString(),
            (possibilities, fixes) => possibilities.SelectMany(x => x.ToCharArray()).GroupBy(x => x).First(x =>
                x.Count() == 9 && fixes[0].ToCharArray().Contains(x.Key) && fixes[2].ToCharArray().Contains(x.Key)).Key.ToString(),
            (possibilities, fixes) => possibilities.SelectMany(x => x.ToCharArray()).GroupBy(x => x).First(x =>
                x.Count() == 7 && !fixes[0].ToCharArray().Contains(x.Key) && !fixes[2].ToCharArray().Contains(x.Key) && !fixes[1].ToCharArray().Contains(x.Key)).Key.ToString()
        };

        private readonly Dictionary<string, string> _digits = new Dictionary<string, string>
        {
            { "012456", "0" },
            { "25", "1" },
            { "02346", "2" },
            { "02356", "3" },
            { "1235", "4" },
            { "01356", "5" },
            { "013456", "6" },
            { "025", "7" },
            { "0123456", "8" },
            { "012356", "9" }
        };
    }
}
