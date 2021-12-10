using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day10
    {
        public int Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            var result = 0;
            foreach(var line in input)
            {
                string treatedLine = TreatLine(line);
                if (IsCorruptedLine(treatedLine))
                {
                    result += _points[treatedLine.First(x => _points.Keys.Contains(x))];
                }
            }
            return result;
        }

        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            var scores = new List<long>();
            foreach (var line in input)
            {
                string treatedLine = TreatLine(line);
                if (IsIncompleteLine(treatedLine))
                {
                    long score = 0;
                    foreach(char c in treatedLine.Reverse())
                    {
                        score = score * 5 + _completePoints[c];
                    }
                    scores.Add(score);
                }
            }
            return scores.OrderBy(x => x).ToList()[scores.Count / 2];
        }

        private string TreatLine(string line)
        {
            var previousLength = line.Length + 1;
            while (line.Length > 0 && line.Length < previousLength)
            {
                previousLength = line.Length;
                for (int i = 0; i < 4; i++)
                {
                    line = line.Replace($"{_completePoints.Keys.ToList()[i]}{_points.Keys.ToList()[i]}", "");
                }
            }
            return line;
        }

        private bool IsCorruptedLine(string line)
        {
            return line.Length > 0 && line.ToCharArray().Any(x => _points.Keys.Contains(x));
        }

        private bool IsIncompleteLine(string line)
        {
            return line.Length > 0 && !line.ToCharArray().Any(x => _points.Keys.Contains(x));
        }

        Dictionary<char, int> _points = new Dictionary<char, int>
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 }
        };

        Dictionary<char, int> _completePoints = new Dictionary<char, int>
        {
            { '(', 1 },
            { '[', 2 },
            { '{', 3 },
            { '<', 4 }
        };
    }
}
