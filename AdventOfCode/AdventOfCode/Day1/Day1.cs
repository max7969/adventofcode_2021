using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day1
    {
        public int Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).Select(int.Parse).ToList();

            return CountIncreasement(input);
        }

        public int Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).Select(int.Parse).ToList();

            List<int> sums = new List<int>();
            for (int i = 0; i < input.Count - 2; i++)
            {
                sums.Add(input[i] + input[i + 1] + input[i + 2]);
            }

            return CountIncreasement(sums);
        }

        private static int CountIncreasement(List<int> input)
        {
            int count = 0;
            for (int i = 1; i < input.Count; i++)
            {
                count = input[i - 1] < input[i] ? count + 1 : count;
            }

            return count;
        }
    }
}
