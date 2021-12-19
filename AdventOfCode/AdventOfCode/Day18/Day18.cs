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
    public class Day18
    {
        public class Action
        {
            public int Type { get; set; }
            public int Order { get; set; }
            public int Level { get; set; }
        }  
        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            string finalChain = SumAll(input);

            return Magnitude(finalChain);
        }

        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            long magnitudeMax = 0;

            foreach(var chain in input)
            {
                foreach(var addedChain in input)
                {
                    if (chain != addedChain)
                    {
                        var result = Magnitude(Add(chain, addedChain));
                        magnitudeMax = result > magnitudeMax ? result : magnitudeMax;
                    }
                }
            }

            return magnitudeMax;
        }

        public static long Magnitude(string chain)
        {
            Regex regex = new Regex(@"\[([0-9]+),([0-9]+)\]");

            while (regex.Matches(chain).Count > 0)
            {
                foreach(Match match in regex.Matches(chain))
                {
                    chain = chain.Replace(match.Value, (long.Parse(match.Groups[1].Value) * 3 + long.Parse(match.Groups[2].Value) * 2).ToString());
                }
            }
            return long.Parse(chain);
        }

        public static string SumAll(List<string> chains)
        {
            string chain = chains[0];
            foreach (string addChain in chains.Skip(1))
            {
                chain = Add(chain, addChain);
            }
            return chain;
        }

        public static string Add(string chain1, string chain2)
        {
            var chain = $"[{chain1},{chain2}]";
            return CycleReduce(chain);
            
        }

        public static string CycleReduce(string chain)
        {
            List<string> reductions = new List<string>();
            var reducedChain = Reduce(chain);
            while (chain != reducedChain)
            {
                chain = reducedChain;
                reducedChain = Reduce(chain);
                reductions.Add(chain);
            }
            return chain;
        }

        public static string Reduce(string chain)
        {
            var chainArray = new List<string>();

            foreach(char c in chain.ToCharArray())
            {
                if (specificChar.Contains(c.ToString()) || specificChar.Contains(chainArray.Last()))
                {
                    chainArray.Add(c.ToString());
                } else
                {
                    chainArray[chainArray.Count -1] += c.ToString(); 
                }
            }

            for (int i=0; i< chainArray.Count; i++)
            {
                int countOpen = chainArray.Where((x, index) => x == "[" && index <= i).Count() - chainArray.Where((x, index) => x == "]" && index <= i).Count();
                if (countOpen > 4 && chainArray[i+2] == "," && chainArray[i+4] == "]")
                {
                    int left = int.Parse(chainArray[i + 1]);
                    int right = int.Parse(chainArray[i + 3]);
                    for (int j = i + 5; j < chainArray.Count; j++)
                    {
                        if (!specificChar.Contains(chainArray[j]))
                        {
                            var newValue = int.Parse(chainArray[j]) + right;
                            chainArray[j] = newValue.ToString();
                            break;
                        }
                    }
                    for (int j=i-1;j>=0;j--)
                    {
                        if (!specificChar.Contains(chainArray[j]))
                        {
                            var newValue = int.Parse(chainArray[j]) + left;
                            chainArray[j] = newValue.ToString();
                            break;
                        }
                    }
                    chainArray[i] = "0";
                    for (int j=0;j<4;j++)
                    {
                        chainArray.RemoveAt(i+1);
                    }
                }
            }

            for (int i= 0; i<chainArray.Count; i++)
            {
                if (chainArray[i].Length > 1)
                {
                    int value = int.Parse(chainArray[i]);
                    chainArray.InsertRange(i + 1, new List<string> { "[", (value / 2).ToString(), ",", ((value / 2) + (value % 2)).ToString(), "]" });
                    chainArray.RemoveAt(i);
                    break;
                }
            }

            return string.Join("", chainArray);
        }

        public static List<string> specificChar = new List<string> { "[", "]", "," };
    }
}
