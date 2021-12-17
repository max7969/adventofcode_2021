using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Markup;
using System.Xml;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day17
    {
        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var results = ComputeLaunches(input);
            return results.Values.Max();
        }

        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            var results = ComputeLaunches(input);
            return results.Values.Count(x => x >= 0);
        }

        private Dictionary<Coordinate, int> ComputeLaunches(List<string> input)
        {
            var coordinates = input[0].Replace("target area: ", "").Split(",");

            var min = new Coordinate
            {
                X = int.Parse(coordinates[0].Replace("x=", "").Split("..")[0]),
                Y = int.Parse(coordinates[1].Replace("y=", "").Split("..")[0])
            };
            var max = new Coordinate
            {
                X = int.Parse(coordinates[0].Replace("x=", "").Split("..")[1]),
                Y = int.Parse(coordinates[1].Replace("y=", "").Split("..")[1])
            };

            Dictionary<Coordinate, int> results = new Dictionary<Coordinate, int>();
            for (int i = 0; i <= max.X; i++)
            {
                for (int j = min.Y; j < Math.Abs(min.Y); j++)
                {
                    Coordinate tested = new Coordinate { X = i, Y = j };
                    results.Add(tested, computePoints(tested, min, max));
                }
            }
            return results;
        }

        private int computePoints(Coordinate velocity, Coordinate min, Coordinate max)
        {
            Coordinate initialPoint = new Coordinate();
            int maxY = initialPoint.Y;
            bool isAreaTouched = false;
            while (initialPoint.X <= max.X && initialPoint.Y >= min.Y)
            {
                initialPoint.X += velocity.X;
                initialPoint.Y += velocity.Y;
                if (initialPoint.X >= min.X && initialPoint.X <= max.X && initialPoint.Y >= min.Y && initialPoint.Y <= max.Y)
                {
                    isAreaTouched = true;
                }
                if (initialPoint.Y > maxY)
                {
                    maxY = initialPoint.Y;
                }

                velocity.X = velocity.X > 0 ? velocity.X - 1 : 0;
                velocity.Y = velocity.Y - 1;
            }
            return isAreaTouched ? maxY : - 1;
        }
    }
}
