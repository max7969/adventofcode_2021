using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day12
    {
        public int Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, List<string>> map = BuildMap(input);
            CleanMap(map);
            List<string> paths = GetPathsFromMap(map);
            return paths.Count();
        }

        public int Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, List<string>> initialMap = BuildMap(input);
            Dictionary<string, Dictionary<string, List<string>>> allMaps = BuildVariantsMaps(initialMap);

            HashSet<string> allPaths = new HashSet<string>();
            foreach (var map in allMaps)
            {
                CleanMap(map.Value);
                List<string> paths = GetPathsFromMap(map.Value);
                paths = paths.Select(x => x.Replace(map.Key + map.Key, map.Key)).ToList();
                foreach (var path in paths)
                {
                    allPaths.Add(path);
                }
            }
            return allPaths.Count;
        }

        private static Dictionary<string, Dictionary<string, List<string>>> BuildVariantsMaps(Dictionary<string, List<string>> initialMap)
        {
            Dictionary<string, Dictionary<string, List<string>>> allMaps = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (var key in initialMap.Keys)
            {
                if (key.ToLower() == key && key != "start" && key != "end")
                {
                    var newMap = new Dictionary<string, List<string>>();
                    foreach (var element in initialMap)
                    {
                        if (element.Key == key)
                        {
                            newMap.Add(key + key, element.Value);
                            newMap.Add(key, element.Value);
                        }
                        else
                        {
                            var copyList = element.Value.ToList();
                            if (element.Value.Contains(key))
                            {
                                copyList.Add(key + key);
                            }
                            newMap.Add(element.Key, copyList);
                        }
                    }
                    allMaps.Add(key, newMap);
                }
            }

            return allMaps;
        }

        private static Dictionary<string, List<string>> BuildMap(List<string> input)
        {
            Dictionary<string, List<string>> map = new Dictionary<string, List<string>>();

            foreach (var line in input)
            {
                var points = line.Split("-");
                if (map.ContainsKey(points[0]))
                {
                    map[points[0]].Add(points[1]);
                }
                else
                {
                    map[points[0]] = new List<string>() { points[1] };
                }

                if (map.ContainsKey(points[1]))
                {
                    map[points[1]].Add(points[0]);
                }
                else
                {
                    map[points[1]] = new List<string>() { points[0] };
                }
            }

            return map;
        }

        private static List<string> GetPathsFromMap(Dictionary<string, List<string>> map)
        {
            List<string> paths = new List<string> { "start" };

            while (!paths.All(x => x.Split(",").Last() == "end"))
            {
                List<string> newPaths = new List<string>();
                foreach (var path in paths)
                {
                    string last = path.Split(",").Last();
                    if (last != "end")
                    {
                        foreach (var move in map[last])
                        {
                            if ((move.ToLower() == move && path.Replace(move, "").Length == path.Length) || move.ToLower() != move)
                            {
                                newPaths.Add(path + "," + move);
                            }
                        }
                    }
                    else
                    {
                        newPaths.Add(path);
                    }
                }
                paths = newPaths.ToList();
            }

            return paths;
        }

        private static void CleanMap(Dictionary<string, List<string>> map)
        {
            List<string> removes = new List<string>();
            foreach (var element in map)
            {
                if (element.Key.ToLower() == element.Key && element.Value.All(x => x.ToLower() == x) && !element.Value.Contains("end") && element.Value.Count < 2 && element.Key != "end")
                {
                    removes.Add(element.Key);
                }
            }

            foreach (var remove in removes)
            {
                map.Remove(remove);
                foreach (var element in map)
                {
                    element.Value.Remove(remove);
                }
            }
        }
    }
}
