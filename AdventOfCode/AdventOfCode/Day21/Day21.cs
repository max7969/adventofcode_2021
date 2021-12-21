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
    public class Day21
    {
        public class Player
        {
            public int Score { get; set; }
            public int Position { get; set; }
        }

        public class PlayerComplex
        {
            public Dictionary<string, double> Score { get; set; }
        }

        public long Compute(string filePath, int steps = 2)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            List<Player> players = new List<Player>();
            players.Add(new Player { Score = 0, Position = int.Parse(input[0].Replace("Player 1 starting position: ", ""))});
            players.Add(new Player { Score = 0, Position = int.Parse(input[1].Replace("Player 2 starting position: ", "")) });

            int turn = 0;
            int diceRoll = 0;
            while (!players.Any(x => x.Score >= 1000))
            {
                int diceValue1 = (turn * 3 + 1) - 100 * (diceRoll / 100);
                diceRoll++;
                int diceValue2 = (turn * 3 + 2) - 100 * (diceRoll / 100);
                diceRoll++;
                int diceValue3 = (turn * 3 + 3) - 100 * (diceRoll / 100);
                diceRoll++;

                int actualIndex = grid.IndexOf(players[turn % 2].Position);
                players[turn % 2].Position = grid[(actualIndex + diceValue1 + diceValue2 + diceValue3) % 10];
                players[turn % 2].Score += players[turn % 2].Position;
                turn++;
            }
            return players.Single(x => x.Score < 1000).Score * diceRoll;
        }

        public double Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            List<PlayerComplex> players = new List<PlayerComplex>();
            players.Add(new PlayerComplex { Score = new Dictionary<string, double>() { { $"0,-1,{int.Parse(input[0].Replace("Player 1 starting position: ", ""))}", 1 } }});
            players.Add(new PlayerComplex { Score = new Dictionary<string, double>() { { $"0,0,{int.Parse(input[1].Replace("Player 2 starting position: ", ""))}", 1 } }});

            List<(int move, long times)> probas = new List<(int move, long times)> { (3, 1), (4, 3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1) };

            for (int i=1; i<20; i++)
            {
                var scoreToChecks = players[(i + 1) % 2].Score.Where(x => x.Key.Split(",")[1] == $"{i - 2}" && int.Parse(x.Key.Split(",")[0]) < 21).ToList();
                var generatedWorldLastTurn = players[i % 2].Score.Where(x => x.Key.Split(",")[1] == $"{i - 1}" && int.Parse(x.Key.Split(",")[0]) < 21).Sum(x => x.Value);
                List<int> addedScore = new List<int>();
                foreach (var score in scoreToChecks)
                {
                    var lastScore = int.Parse(score.Key.Split(",")[0]);
                    var lastPosition = int.Parse(score.Key.Split(",")[2]);
                    var lastIndex = grid.IndexOf(lastPosition);
                    foreach (var proba in probas)
                    {
                        addedScore.Add(grid[(lastIndex + proba.move) % 10] + lastScore);
                        var key = $"{grid[(lastIndex + proba.move) % 10] + lastScore},{i},{grid[(lastIndex + proba.move) % 10]}";
                        if (players[(i + 1) % 2].Score.ContainsKey(key))
                        {
                            players[(i + 1) % 2].Score[key] += score.Value * (generatedWorldLastTurn * proba.times / scoreToChecks.Sum(x => x.Value));
                        } else
                        {
                            players[(i + 1) % 2].Score.Add($"{grid[(lastIndex + proba.move) % 10] + lastScore},{i},{grid[(lastIndex + proba.move) % 10]}", score.Value * (generatedWorldLastTurn * proba.times / scoreToChecks.Sum(x => x.Value)));
                        }
                    }
                }

                if (addedScore.All(x => x >= 21))
                {
                    break;
                }
            }

            var potentialWinPlayer1 = players[0].Score.Where(x => int.Parse(x.Key.Split(",")[0]) >= 21).ToList();
            var potentialWinPlayer2 = players[1].Score.Where(x => int.Parse(x.Key.Split(",")[0]) >= 21).ToList();

            return new List<double> { potentialWinPlayer1.Sum(x => x.Value), potentialWinPlayer2.Sum(x => x.Value) }.Max();
        }

        public List<int> grid = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    }
}
