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
            public Coordinate Coordinate { get; set; }
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
            BuildIntersections(scanners, intersections);

            int sum = 0;
            var all = scanners.SelectMany(x => x.BeaconsIntersections.Values).ToList();

            foreach (var value in all.ToHashSet())
            {
                sum += all.Count(x => x == value) / value;
            }

            return sum;
        }

        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            List<Scanner> scanners = ExtractScanners(input);
            List<string> intersections = new List<string>();
            BuildIntersections(scanners, intersections);

            while(scanners.Any(x => x.Coordinate == null))
            {
                var knownPositions = scanners.Where(x => x.Coordinate != null).ToList();
                var unknownPositions = scanners.Where(x => x.Coordinate == null).ToList();

                foreach(var intersection in intersections)
                {
                    int a = int.Parse(intersection.Split(",")[0]);
                    int b = int.Parse(intersection.Split(",")[1]);

                    if (knownPositions.Select(x => x.Number).Contains(a) && unknownPositions.Select(x => x.Number).Contains(b))
                    {
                        var known = scanners.Single(x => x.Number == a);
                        var unknown = scanners.Single(x => x.Number == b);

                        var matchingDistances = known.Distances.Keys.Intersect(unknown.Distances.Keys).ToList();
                        HashSet<int> beaconInI = new HashSet<int>();
                        HashSet<int> beaconInJ = new HashSet<int>();
                        List<(Coordinate, Coordinate)> beaconsCouple = new List<(Coordinate, Coordinate)>();
                        foreach (var matchingDistance in matchingDistances)
                        {
                            foreach (var element in known.Distances[matchingDistance].Split(",").Select(int.Parse).ToList())
                            {
                                beaconInI.Add(element);
                            }
                            foreach (var element in unknown.Distances[matchingDistance].Split(",").Select(int.Parse).ToList())
                            {
                                beaconInJ.Add(element);
                            }
                        }

                        for (int i = 0; i < beaconInI.Count; i++)
                        {
                            beaconsCouple.Add((known.Beacons[beaconInI.ToList()[i]], unknown.Beacons[beaconInJ.ToList()[i]]));
                        }

                        int? xValue = FindSolution(beaconsCouple.Select(x => x.Item1.X).ToList(), beaconsCouple.Select(x => x.Item2.X).ToList());

                        xValue = xValue ?? FindSolution(beaconsCouple.Select(x => x.Item1.X).ToList(), beaconsCouple.Select(x => -x.Item2.X).ToList());
                        xValue = xValue ?? FindSolution(beaconsCouple.Select(x => x.Item1.X).ToList(), beaconsCouple.Select(x => x.Item2.Y).ToList());
                        xValue = xValue ?? FindSolution(beaconsCouple.Select(x => x.Item1.X).ToList(), beaconsCouple.Select(x => x.Item2.Z).ToList());

                        int? yValue = FindSolution(beaconsCouple.Select(x => x.Item1.Y).ToList(), beaconsCouple.Select(x => x.Item2.Y).ToList());
                        yValue = yValue ?? FindSolution(beaconsCouple.Select(x => x.Item1.Y).ToList(), beaconsCouple.Select(x => x.Item2.X).ToList());
                        yValue = yValue ?? FindSolution(beaconsCouple.Select(x => x.Item1.Y).ToList(), beaconsCouple.Select(x => x.Item2.Z).ToList());

                        int? zValue = FindSolution(beaconsCouple.Select(x => x.Item1.Z).ToList(), beaconsCouple.Select(x => x.Item2.Z).ToList());
                        zValue = zValue ?? FindSolution(beaconsCouple.Select(x => x.Item1.Z).ToList(), beaconsCouple.Select(x => x.Item2.X).ToList());
                        zValue = zValue ?? FindSolution(beaconsCouple.Select(x => x.Item1.Z).ToList(), beaconsCouple.Select(x => x.Item2.Y).ToList());

                        unknown.Coordinate = new Coordinate {X = known.Coordinate.X + xValue ?? 0, Y= known.Coordinate.Y + yValue ?? 0, Z = known.Coordinate.Z + zValue ?? 0};
                    }
                }
            }

            return 0;
        }

        public int? FindSolution(List<int> a, List<int> b)
        {
            HashSet<int> results = a.Select((x, i) => x + b[i]).ToHashSet();
            if (results.Count == 1)
            {
                return results.First();
            }
            return null;
        }

        private static void BuildIntersections(List<Scanner> scanners, List<string> intersections)
        {
            for (int i = 0; i < scanners.Count; i++)
            {
                for (int j = i + 1; j < scanners.Count; j++)
                {
                    var matchingDistances = scanners[i].Distances.Keys.Intersect(scanners[j].Distances.Keys).ToList();

                    HashSet<int> beaconInI = new HashSet<int>();
                    HashSet<int> beaconInJ = new HashSet<int>();

                    foreach (var matchingDistance in matchingDistances)
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
            scanners[0].Coordinate = new Coordinate();
            return scanners;
        }

        private int ComputeDist(int xA, int xB, int yA, int yB)
        {
            return Math.Abs(xA - xB) + Math.Abs(yA - yB);
        }
    }
}
