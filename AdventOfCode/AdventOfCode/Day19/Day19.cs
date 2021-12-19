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
    public class Day19
    {
        public class Scanner
        {
            public int Number { get; set; }
            public List<Coordinate> Beacons { get; set; } = new List<Coordinate>();
            public Dictionary<string, string> Distances { get; set; } = new Dictionary<string, string>();
            public Dictionary<int, int> BeaconsIntersections { get; set; } = new Dictionary<int, int>();
        }

        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
        }

        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            
            List<Scanner> scanners = ExtractScanners(input);

            List<string> intersections = new List<string>();

            for (int i = 0;i < scanners.Count; i++)
            {
                for (int j = i+1;j< scanners.Count;j++)
                {
                    var matchingDistances = scanners[i].Distances.Keys.Intersect(scanners[j].Distances.Keys).ToList();

                    HashSet<int> beaconInI = new HashSet<int>();
                    HashSet<int> beaconInJ = new HashSet<int>();

                    foreach(var matchingDistance in matchingDistances)
                    {
                        foreach (var element in scanners[i].Distances[matchingDistance].Split(",").Select(int.Parse).ToList())
                        {
                            beaconInI.Add(element);
                        }
                        foreach (var element in scanners[j].Distances[matchingDistance].Split(",").Select(int.Parse).ToList())
                        {
                            beaconInJ.Add(element);
                        }
                    }

                    foreach (var element in beaconInI)
                    {
                        scanners[i].BeaconsIntersections[element] += 1;
                    }
                    foreach (var element in beaconInJ)
                    {
                        scanners[j].BeaconsIntersections[element] += 1;
                    }

                    if (matchingDistances.Count() >= 66)
                    {
                        intersections.Add($"{i},{j}");
                    }
                }
            }

            int sum = 0;
            var all = scanners.SelectMany(x => x.BeaconsIntersections.Values).ToList();

            foreach (var value in all.ToHashSet())
            {
                sum += all.Count(x => x == value) / value;
            }


            return sum;
        }

        private List<Scanner> ExtractScanners(List<string> input)
        {
            List<Scanner> scanners = new List<Scanner>();
            Regex regexScanner = new Regex(@"--- scanner ([0-9]+) ---");
            Regex regexCoordinate = new Regex(@"([\-]*[0-9]+),([\-]*[0-9]+),([\-]*[0-9]+)");

            foreach(var line in input)
            {
                if (regexScanner.Match(line).Success)
                {
                    scanners.Add(new Scanner { Number = int.Parse(regexScanner.Match(line).Groups[1].Value) });
                } else if (!string.IsNullOrEmpty(line))
                {
                    var axes = regexCoordinate.Match(line).Groups;

                    var newBeacon = new Coordinate { X = int.Parse(axes[1].Value), Y = int.Parse(axes[2].Value), Z = int.Parse(axes[3].Value) };

                    var actualBeaconCount = scanners.Last().Beacons.Count;
                    for (int i=0;i< actualBeaconCount; i++)
                    {
                        List<int> distances = new List<int>();
                        distances.Add(ComputeDist(scanners.Last().Beacons[i].X, newBeacon.X, scanners.Last().Beacons[i].Y, newBeacon.Y));
                        distances.Add(ComputeDist(scanners.Last().Beacons[i].X, newBeacon.X, scanners.Last().Beacons[i].Z, newBeacon.Z));
                        distances.Add(ComputeDist(scanners.Last().Beacons[i].Z, newBeacon.Z, scanners.Last().Beacons[i].Y, newBeacon.Y));
                        scanners.Last().Distances.Add(String.Join(",", distances.OrderBy(x => x)), $"{i},{actualBeaconCount}");
                    }
                    scanners.Last().Beacons.Add(newBeacon);
                    
                }
            }

            foreach (var scanner in scanners)
            {
                for (int i = 0; i < scanner.Beacons.Count; i++)
                {
                    scanner.BeaconsIntersections.Add(i, 1);
                }
            }
            return scanners;
        }

        private int ComputeDist(int xA, int xB, int yA, int yB)
        {
            return Math.Abs(xA - xB) + Math.Abs(yA - yB);
        }
    }
}
