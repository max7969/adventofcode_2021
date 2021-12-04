using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day3
    {
      public int Compute(string filePath)
      {
          var input = FileReader.GetFileContent(filePath).ToList();


          var entries = input.Select(x => x.ToCharArray().Select(x => int.Parse(x.ToString())).ToList());
          int lenght = input[0].Length;

          string gamma = "";
          string epsilon = "";

          for (int i = 0; i < lenght; i++)
          {
              bool isOneMostRepresented = entries.Select(x => x[i]).Sum() > entries.Count() / 2.0;
              gamma += isOneMostRepresented ? 1 : 0;
              epsilon += isOneMostRepresented ? 0 : 1;
          }

          return Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2);
      }

      public int Compute2(string filePath)
      {
          var input = FileReader.GetFileContent(filePath).ToList();

          var entries = input.Select(x => x.ToCharArray().Select(x => int.Parse(x.ToString())).ToList());
          int lenght = input[0].Length;

          string oxygenRating = FilteringMost(lenght, entries);
          string co2Rating = FilteringLess(lenght, entries);

          return Convert.ToInt32(oxygenRating, 2) * Convert.ToInt32(co2Rating, 2);
        }

      private static string FilteringMost(int lenght, IEnumerable<List<int>> entries)
      {
          var copy = entries.ToList();
          for (int i = 0; i < lenght; i++)
          {
              bool isOneMostRepresented = copy.Select(x => x[i]).Sum() >= copy.Count() / 2.0;

              copy = copy.Where(x => x[i] == (isOneMostRepresented ? 1 : 0)).ToList();

              if (copy.Count == 1)
              {
                  break;
              }
            }

          return string.Concat(copy[0].Select(x => x.ToString()).ToArray());
      }

      private static string FilteringLess(int lenght, IEnumerable<List<int>> entries)
      {
          var copy = entries.ToList();
          for (int i = 0; i < lenght; i++)
          {
              bool isOneMostRepresented = copy.Select(x => x[i]).Sum() >= copy.Count() / 2.0;

              copy = copy.Where(x => x[i] == (isOneMostRepresented ? 0 : 1)).ToList();

              if (copy.Count == 1)
              {
                  break;
              }
          }

          return string.Concat(copy[0].Select(x => x.ToString()).ToArray());
      }
    }
}
