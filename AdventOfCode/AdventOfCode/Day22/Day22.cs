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
    public class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public bool IsEqual(Coordinate coord)
        {
            return this.X == coord.X && this.Y == coord.Y && this.Z == coord.Z;
        }
    }

    public class Cube
    {
        public Coordinate Corner1 {get;set;}
        public Coordinate Corner2 {get;set;}
        public bool Lighted { get; set; }

        public bool IsEqual(Cube test)
        {
            return this.Corner1.IsEqual(test.Corner1) && this.Corner2.IsEqual(test.Corner2) && this.Lighted == test.Lighted;
        }
    }

    public class Day22
    {
        public double Compute(string filePath, bool removeOver50 = true)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            List<Cube> initialCubes = new List<Cube>();
            var lineRegex = new Regex(@"([onf]+) x=([\-0-9]+)..([\-0-9]+),y=([\-0-9]+)..([\-0-9]+),z=([\-0-9]+)..([\-0-9]+)");

            foreach (var line in input)
            {
                var groups = lineRegex.Match(line).Groups;

                bool toAdd = true;
                if (removeOver50)
                {
                    for (int i=2;i<=7;i++)
                    {
                        if (Math.Abs(int.Parse(groups[i].Value)) > 50)
                        {
                            toAdd = false;
                        }
                    }
                }
                if (toAdd)
                {
                    initialCubes.Add(new Cube
                    {
                        Lighted = groups[1].Value == "on",
                        Corner1 = new Coordinate
                        {
                            X = double.Parse(groups[2].Value) - 0.5,
                            Y = double.Parse(groups[4].Value) - 0.5,
                            Z = double.Parse(groups[6].Value) - 0.5
                        },
                        Corner2 = new Coordinate
                        {
                            X = double.Parse(groups[3].Value) + 0.5,
                            Y = double.Parse(groups[5].Value) + 0.5,
                            Z = double.Parse(groups[7].Value) + 0.5
                        }
                    });
                }
            }

            List<Cube> finalCubes = new List<Cube> { initialCubes[0] };
            List<Cube> temp = new List<Cube>();
            for (int i = 1; i<initialCubes.Count; i++)
            {
                var cubeB = initialCubes[i];
                var overlappingCubes = finalCubes.Where(x => IsOverlaping(x, cubeB)).ToList();
                finalCubes = finalCubes.Where(x => !IsOverlaping(x, cubeB)).ToList();

                if (overlappingCubes.Any())
                {
                    foreach (var cubeA in overlappingCubes)
                    {
                        var addedCubes = AddCubes(cubeA, cubeB);
                        foreach (var addCube in addedCubes)
                        {
                            if (!finalCubes.Any(x => IsInside(addCube, x))) {
                                finalCubes.Add(addCube);
                            }
                        }
                    }
                } 
                else if (cubeB.Lighted)
                {
                    finalCubes.Add(cubeB);
                }
                finalCubes = finalCubes.OrderBy(x => CubeSize(x)).Where((x, i) => !finalCubes.OrderBy(x => CubeSize(x)).Where((y, j) => j > i).Any(y => IsInside(x, y))).ToList();
                finalCubes = Reduce(finalCubes);
                finalCubes = finalCubes.Where(x => x.Corner1.X != x.Corner2.X && x.Corner1.Y != x.Corner2.Y && x.Corner1.Z != x.Corner2.Z).ToList();
                finalCubes = GroupMiniCubes(finalCubes);
            }
            finalCubes.AddRange(temp);
            finalCubes = Reduce(finalCubes);

            return finalCubes.Sum(x => CubeSize(x));
        }

        private List<Cube> Reduce(List<Cube> finalCubes)
        {
            while(IsAnyOverlap(finalCubes))
            {
                var allSelected = finalCubes.Where(x => finalCubes.Any(y => !x.IsEqual(y) && IsOverlaping(x, y))).ToList();

                foreach (var selected in allSelected)
                {
                    var overlapping = finalCubes.Where(x => !x.IsEqual(selected) && IsOverlaping(x, selected)).ToList();
                    foreach(var overlap in overlapping)
                    {
                        finalCubes = finalCubes.Where(x => !x.IsEqual(overlap) && !x.IsEqual(selected)).ToList();
                        var addedCubes = AddCubes(selected, overlap);
                        foreach (var addCube in addedCubes)
                        {
                            if (!finalCubes.Any(x => IsInside(addCube, x)))
                            {
                                finalCubes.Add(addCube);
                            }
                        }
                    }
                }
            }
            return finalCubes;
        }

        public List<Cube> GroupMiniCubes(List<Cube> cubes)
        {
            var miniCubes = cubes.Where(x => x.Lighted);
            foreach (var cube in miniCubes)
            {
                var friend = cubes.SingleOrDefault(x => x.Corner1.X == cube.Corner2.X 
                && x.Corner1.Y == cube.Corner1.Y && x.Corner2.Y == cube.Corner2.Y 
                && x.Corner1.Z == cube.Corner1.Z && x.Corner2.Z == cube.Corner2.Z);
                if (friend != null)
                {
                    cubes = cubes.Where(x => !x.IsEqual(friend) && !x.IsEqual(cube)).ToList();
                    cubes.Add(new Cube { Lighted = true, Corner1 = cube.Corner1, Corner2 = friend.Corner2 });
                }
            }
            miniCubes = cubes.Where(x => x.Lighted);
            foreach (var cube in miniCubes)
            {
                var friend = cubes.SingleOrDefault(x => x.Corner1.X == cube.Corner1.X && x.Corner2.X == cube.Corner2.X
                && x.Corner1.Y == cube.Corner2.Y
                && x.Corner1.Z == cube.Corner1.Z && x.Corner2.Z == cube.Corner2.Z); 
                if (friend != null)
                {
                    cubes = cubes.Where(x => !x.IsEqual(friend) && !x.IsEqual(cube)).ToList();
                    cubes.Add(new Cube { Lighted = true, Corner1 = cube.Corner1, Corner2 = friend.Corner2 });
                }
            }
            miniCubes = cubes.Where(x => x.Lighted);
            foreach (var cube in miniCubes)
            {
                var friend = cubes.SingleOrDefault(x => x.Corner1.X == cube.Corner1.X && x.Corner2.X == cube.Corner2.X
                && x.Corner1.Y == cube.Corner1.Y && x.Corner2.Y == cube.Corner2.Y
                && x.Corner1.Z == cube.Corner2.Z);
                if (friend != null)
                {
                    cubes = cubes.Where(x => !x.IsEqual(friend) && !x.IsEqual(cube)).ToList();
                    cubes.Add(new Cube { Lighted = true, Corner1 = cube.Corner1, Corner2 = friend.Corner2 });
                }
            }
            return cubes;
        }

        public bool IsAnyOverlap(List<Cube> cubes)
        {
            for (int i = 0; i < cubes.Count; i++)
            {
                for (int j = 0; j < cubes.Count; j++)
                {
                    if (i != j && IsOverlaping(cubes[i], cubes[j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public double CubeSize(Cube cube)
        {
            return Math.Abs(cube.Corner2.X - cube.Corner1.X) * Math.Abs(cube.Corner2.Y - cube.Corner1.Y) * Math.Abs(cube.Corner2.Z - cube.Corner1.Z);
        }

        public List<Cube> AddCubes(Cube a, Cube b)
        {
            List<Cube> result = new List<Cube>();
            List<Cube> cutX = new List<Cube>();
            List<Cube> cutY = new List<Cube>();
            List<Cube> cutZ = new List<Cube>();

            List<double> xS = new List<double> { a.Corner1.X, b.Corner1.X, a.Corner2.X, b.Corner2.X }.OrderBy(x => x).ToList();
            List<double> yS = new List<double> { a.Corner1.Y, b.Corner1.Y, a.Corner2.Y, b.Corner2.Y }.OrderBy(x => x).ToList();
            List<double> zS = new List<double> { a.Corner1.Z, b.Corner1.Z, a.Corner2.Z, b.Corner2.Z }.OrderBy(x => x).ToList();

            if (IsInside(a, b) && b.Lighted)
            {
                result.Add(b);
                return result;
            }
            if (IsInside(b, a) && b.Lighted)
            {
                result.Add(a);
                return result;
            }

            for (int i = 0; i < 3; i++)
            {
                cutX.Add(new Cube
                {
                    Lighted = true,
                    Corner1 = new Coordinate { X = xS[i], Y = yS[0], Z = zS[0] },
                    Corner2 = new Coordinate { X = xS[i+1], Y = yS[3], Z = zS[3] }
                });
            }
            foreach (var cut in cutX)
            {
                for (int i = 0; i < 3; i++)
                {
                    cutY.Add(new Cube
                    {
                        Lighted = true,
                        Corner1 = new Coordinate { X = cut.Corner1.X, Y = yS[i], Z = cut.Corner1.Z },
                        Corner2 = new Coordinate { X = cut.Corner2.X, Y = yS[i + 1], Z = cut.Corner2.Z }
                    });
                }
            }
            foreach (var cut in cutY)
            {
                for (int i = 0; i < 3; i++)
                {
                    cutZ.Add(new Cube
                    {
                        Lighted = true,
                        Corner1 = new Coordinate { X = cut.Corner1.X, Y = cut.Corner1.Y, Z = zS[i] },
                        Corner2 = new Coordinate { X = cut.Corner2.X, Y = cut.Corner2.Y, Z = zS[i+1] }
                    });
                }
            }
            if (a.Lighted && b.Lighted)
            {
                result.Add(a);
                result.AddRange(cutZ.Where(x => IsInside(x, b) && !IsInside(x, a)).ToList());
               
            } 
            else
            {
                result.AddRange(cutZ.Where(x => IsInside(x, a) && !IsInside(x, b) && a.Lighted).ToList());
                result.AddRange(cutZ.Where(x => !IsInside(x, a) && IsInside(x, b) && b.Lighted).ToList());
                result.AddRange(cutZ.Where(x => IsInside(x, a) && IsInside(x, b) && b.Lighted).ToList());
            }
            
            return result;
        }

        public bool IsOverlaping(Cube a, Cube b)
        {
            return a.Corner2.X > b.Corner1.X &&
                a.Corner1.X < b.Corner2.X &&
                a.Corner2.Y > b.Corner1.Y &&
                a.Corner1.Y < b.Corner2.Y &&
                a.Corner2.Z > b.Corner1.Z &&
                a.Corner1.Z < b.Corner2.Z;
        }

        public bool IsInside(Cube a, Cube b)
        {
            return ((a.Corner1.X >= b.Corner1.X && a.Corner1.X <= b.Corner2.X) &&
                (a.Corner2.X >= b.Corner1.X && a.Corner2.X <= b.Corner2.X)) &&
                ((a.Corner1.Y >= b.Corner1.Y && a.Corner1.Y <= b.Corner2.Y) &&
                (a.Corner2.Y >= b.Corner1.Y && a.Corner2.Y <= b.Corner2.Y)) &&
                ((a.Corner1.Z >= b.Corner1.Z && a.Corner1.Z <= b.Corner2.Z) &&
                (a.Corner2.Z >= b.Corner1.Z && a.Corner2.Z <= b.Corner2.Z));
        }
    }
}
