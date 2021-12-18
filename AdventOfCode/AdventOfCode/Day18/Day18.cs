using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Markup;
using System.Xml;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day18
    {
        public class Node
        {
            public int Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public int Level { get; set; }
        }
        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            Node node = CreateNodeFromString(input[0], 1);

            return 0;
        }

        private static void Reduce(Node node)
        {
            if (node.Level > 4 && node.Left.Value > 0 && node.Right.Value > 0)
            {

            }
        }

        private static Node CreateNodeFromString(string chain, int level)
        {
            Node node = new Node();
            node.Level = level;

            if (chain.Length == 1)
            {
                node.Value = int.Parse(chain);
                return node;
            }

            var asArray = chain.ToCharArray().ToList();
            asArray.RemoveAt(0);
            asArray.RemoveAt(asArray.Count - 1);

            int countOpen = 0;
            int countClose = 0;
            string left = "";
            string right = "";
            for (int i=0; i<asArray.Count; i++)
            {
                if (asArray[i] == '[')
                {
                    countOpen++;
                } else if (asArray[i] == ']')
                {
                    countClose++;
                }
                if (node.Left == null)
                {
                    left += asArray[i].ToString();
                } else
                {
                    right += asArray[i].ToString();
                }
                if (countOpen == countClose)
                {
                    if (node.Left == null)
                    {
                        node.Left = CreateNodeFromString(left, node.Level + 1);
                        i++;
                    } else
                    {
                        node.Right = CreateNodeFromString(right, node.Level + 1);
                    }
                }
            }
            return node;
        }
    }
}
