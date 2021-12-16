using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day15
    {

        private class Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Cost { get;set; }
            public int Heuristic { get; set; }
        }

        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var grid = input.Select(x => x.ToCharArray().Select(char.ToString).Select(int.Parse).ToList()).ToList();

            int maxX = grid[0].Count() - 1;
            int maxY = grid.Count() - 1;
            Node current = ApplyAStar(grid, maxX, maxY);

            return current == null ? 0 : current.Cost;
        }

        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var grid = input.Select(x => x.ToCharArray().Select(char.ToString).Select(int.Parse).ToList()).ToList();

            int initialMaxX = grid[0].Count() - 1;
            int initialMaxY = grid.Count() - 1;

            for (int i = 0; i<4; i++)
            {
                grid.AddRange(grid.Where((x, index) => index >= i * (initialMaxY + 1) && index <= i * (initialMaxY + 1) + initialMaxY).Select(x => x.ToList()).ToList());

            }

            foreach(var line in grid)
            {
                for (int i = 0; i < 4; i++)
                {
                    line.AddRange(line.Where((x, index) => index >= i * (initialMaxX + 1) && index <= i * (initialMaxX + 1) + initialMaxX).ToList());
                }
            }

            for (int i=0; i<grid.Count(); i++)
            {
                for (int j=0; j<grid[i].Count(); j++)
                {
                    var increaseY = i / (initialMaxY + 1);
                    var increaseX = j / (initialMaxX + 1);
                    grid[i][j] = grid[i][j] + increaseX + increaseY;
                }
            }

            for (int i = 0; i < grid.Count(); i++)
            {
                for (int j = 0; j < grid[i].Count(); j++)
                {
                    if (grid[i][j] >= 10)
                    {
                        grid[i][j] = grid[i][j] % 10 + 1;
                    }
                }
            }

            int maxX = grid[0].Count() - 1;
            int maxY = grid.Count() - 1;
            Node current = ApplyAStar(grid, maxX, maxY);

            return current == null ? 0 : current.Cost;
        }

        private static Node ApplyAStar(List<List<int>> grid, int maxX, int maxY)
        {
            HashSet<string> closedList = new HashSet<string>();
            List<Node> openList = new List<Node>();
            openList.Add(new Node() { X = 0, Y = 0, Cost = 0, Heuristic = 0 });

            Node current = null;
            while (openList.Count > 0)
            {
                current = openList.First();
                openList.Remove(current);

                if (current.X == maxX && current.Y == maxY)
                {
                    break;
                }
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (current.X + i >= 0 && current.X + i <= maxX && current.Y + j >= 0 && current.Y + j <= maxY && (i != 0 || j != 0) && (j == 0 || i == 0))
                        {
                            var newNode = new Node
                            {
                                X = current.X + i,
                                Y = current.Y + j,
                                Cost = current.Cost + grid[current.Y + j][current.X + i],
                                Heuristic = current.Cost + grid[current.Y + j][current.X + i] + Math.Abs(maxX - (current.X + i)) + Math.Abs(maxY - (current.Y + j))
                            };
                            if (!(closedList.Contains(newNode.X + "," + newNode.Y) || openList.Any(x => x.X == newNode.X && x.Y == newNode.Y && x.Cost <= newNode.Cost)))
                            {
                                openList.Add(newNode);
                            }
                        }
                    }
                }
                closedList.Add(current.X + "," + current.Y);
                openList = openList.OrderBy(x => x.Heuristic).ToList();
            }

            return current;
        }
    }
}
