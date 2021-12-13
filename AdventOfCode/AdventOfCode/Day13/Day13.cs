using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day13
    {
        public int Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            HashSet<string> points = new HashSet<string>();
            List<string> moves = new List<string>();
            foreach(var line in input)
            {
                if (line.Contains(","))
                {
                    points.Add(line);
                } else if (line.Contains("fold along"))
                {
                    moves.Add(line.Split("fold along ")[1]);
                }
            }
            points = Fold(points, moves[0]);
            return points.Count;
        }

        public List<string> Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            HashSet<string> points = new HashSet<string>();
            List<string> moves = new List<string>();
            foreach (var line in input)
            {
                if (line.Contains(","))
                {
                    points.Add(line);
                }
                else if (line.Contains("fold along"))
                {
                    moves.Add(line.Split("fold along ")[1]);
                }
            }

            foreach (var move in moves)
            {
                points = Fold(points, move);
            }

            List<string> draw = Draw(points);

            return draw;
        }

        private static List<string> Draw(HashSet<string> points)
        {
            var allPoints = points.Select(x => x.Split(",").Select(int.Parse).ToArray()).ToList();
            var draw = new List<string>();
            for (int j = 0; j <= allPoints.Select(x => x[1]).Max(); j++)
            {
                string line = "";
                for (int i = 0; i <= allPoints.Select(x => x[0]).Max(); i++)
                {
                    if (points.Contains(i + "," + j))
                    {
                        line += "#";
                    }
                    else
                    {
                        line += " ";
                    }
                }
                draw.Add(line);
            }

            return draw;
        }

        private static HashSet<string> Fold(HashSet<string> points, string move)
        {
            bool isXCut = move.Split("=")[0] == "x";
            HashSet<string> newPoints = new HashSet<string>();
            foreach (var point in points)
            {
                int x = int.Parse(point.Split(",")[0]);
                int y = int.Parse(point.Split(",")[1]);
                int cut = int.Parse(move.Split("=")[1]);
                if (x > cut && isXCut)
                {
                    newPoints.Add((cut * 2 - x) + "," + y);
                }
                else if (y > cut && !isXCut)
                {
                    newPoints.Add(x + "," + (cut * 2 - y));
                }
                else
                {
                    newPoints.Add(point);
                }
            }
            return newPoints;
        }
    }
}
