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

            while (scanners.Any(x => x.Coordinate == null))
            {
                foreach (var intersection in intersections)
                {
                    var knownPositions = scanners.Where(x => x.Coordinate != null).ToList();
                    var unknownPositions = scanners.Where(x => x.Coordinate == null).ToList();

                    int a = int.Parse(intersection.Split(",")[0]);
                    int b = int.Parse(intersection.Split(",")[1]);

                    if (knownPositions.Select(x => x.Number).Contains(a) && unknownPositions.Select(x => x.Number).Contains(b))
                    {
                        FindUnknownCoordinates(scanners, a, b);
                    } 
                    else if (knownPositions.Select(x => x.Number).Contains(b) && unknownPositions.Select(x => x.Number).Contains(a))
                    {
                        FindUnknownCoordinates(scanners, b, a);
                    }
                }
            }

            List<int> distances = new List<int>();
            for (int i=0; i<scanners.Count; i++)
            {
                for (int j=0;j<scanners.Count;j++)
                {
                    distances.Add(Compute3dDist(scanners[i].Coordinate, scanners[j].Coordinate));
                }
            }
            return distances.Max();
        }

        private void FindUnknownCoordinates(List<Scanner> scanners, int a, int b)
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

            foreach (var operation in operations)
            {
                var tupleList = beaconsCouple.Select(x => (x.Item1.X, x.Item1.Y, x.Item1.Z)).ToList();
                var movedTupleList = beaconsCouple.Select(x => operation(x.Item2.X, x.Item2.Y, x.Item2.Z)).ToList();
                var values = FindSolution(tupleList.Select(x => x.X).ToList(), tupleList.Select(x => x.Y).ToList(), tupleList.Select(x => x.Z).ToList(), movedTupleList.Select(x => x.X).ToList(), movedTupleList.Select(x => x.Y).ToList(), movedTupleList.Select(x => x.Z).ToList());
                if (values.xValue != null)
                {
                    unknown.Coordinate = new Coordinate { X = (values.xValue ?? 0), Y = (values.yValue ?? 0), Z = (values.zValue ?? 0) };
                    for (int i = 0; i < unknown.Beacons.Count; i++)
                    {
                        var newCoordinates = operation(unknown.Beacons[i].X, unknown.Beacons[i].Y, unknown.Beacons[i].Z);
                        unknown.Beacons[i].X = newCoordinates.X + unknown.Coordinate.X;
                        unknown.Beacons[i].Y = newCoordinates.Y + unknown.Coordinate.Y;
                        unknown.Beacons[i].Z = newCoordinates.Z + unknown.Coordinate.Z;

                    }
                    break;
                }
            }
        }

        public static List<Func<int, int, int, (int X, int Y, int Z)>> operations = new List<Func<int, int, int, (int X, int Y, int Z)>>
        {
            (x,y,z) => (x, z, -y),
            (x,y,z) => (x, -y, -z),
            (x,y,z) => (x, y, z),
            (x,y,z) => (x, -z, y),
            (x,y,z) => (-z, x, -y),
            (x,y,z) => (-x, -z, -y),
            (x,y,z) => (z, -x, -y),
            (x,y,z) => (z, -y, x),
            (x,y,z) => (y, z, x),
            (x,y,z) => (-z, y, x),
            (x,y,z) => (-y, -z, x),
            (x,y,z) => (-y, x, z),
            (x,y,z) => (-x, -y, z),
            (x,y,z) => (y, -x, z),
            (x,y,z) => (-z, -x, y),
            (x,y,z) => (z, x, y),
            (x,y,z) => (-x, z, y),
            (x,y,z) => (-x, y, -z),
            (x,y,z) => (-y, -x, -z),
            (x,y,z) => (y, x, -z),
            (x,y,z) => (y, -z, -x),
            (x,y,z) => (z, y, -x),
            (x,y,z) => (-y, z, -x),
            (x,y,z) => (-z, -y, -x)
        };

        public (int? xValue, int? yValue, int? zValue) FindSolution(List<int> xA, List<int> yA, List<int> zA, List<int> xB, List<int> yB, List<int> zB)
        {

            Dictionary<int, int> resultsX = xA.Select((x, i) => x - xB[i]).GroupBy(x => x).ToDictionary(x => x.Key, x => x.ToList().Count());
            Dictionary<int, int> resultsY = yA.Select((y, i) => y - yB[i]).GroupBy(x => x).ToDictionary(x => x.Key, x => x.ToList().Count());
            Dictionary<int, int> resultsZ = zA.Select((z, i) => z - zB[i]).GroupBy(x => x).ToDictionary(x => x.Key, x => x.ToList().Count());
            if (resultsX.Values.Max() > 8 && resultsY.Values.Max() > 8 && resultsZ.Values.Max() > 8)
            {
                return (resultsX.Keys.Single(x => resultsX[x] == resultsX.Values.Max()),
                    resultsY.Keys.Single(x => resultsY[x] == resultsY.Values.Max()),
                    resultsZ.Keys.Single(x => resultsZ[x] == resultsZ.Values.Max()));
            }
            return (null, null, null);
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
                        intersections.Add($"{scanners[i].Number},{scanners[j].Number}");
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

        private int Compute3dDist(Coordinate a, Coordinate b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
        }
    }
}
