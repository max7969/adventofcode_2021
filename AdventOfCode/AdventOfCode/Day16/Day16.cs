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
    public class Day16
    {
        public class Element
        {
            public int Version { get; set; }
            public int Type { get; set; }
            public string Value { get; set; }
            public int Size { get; set; }
            public List<Element> SubElements { get; set; } = new List<Element>();
        }

        public Element TreatHexadecimal(string input)
        {
            var binaryChain = "";
            var asStringArray = input.ToCharArray().Select(char.ToString).ToArray();
            foreach (var hexaElement in asStringArray)
            {
                var binaryElement = Convert.ToString(Convert.ToInt32(hexaElement, 16), 2);
                int diff = 4 - binaryElement.Length;
                for (int i = 0; i < diff; i++)
                {
                    binaryElement = "0" + binaryElement;
                }
                binaryChain += binaryElement;
            }
            

            Element element = null;
            
            element = ReadElement(binaryChain);

            return element;
        }

        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            var element = TreatHexadecimal(input[0]);
            return AddUpVersion(element);
        }

        public string Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            var element = TreatHexadecimal(input[0]);
            return element.Value;
        }

        private long AddUpVersion(Element element)
        {
            long sum = element.Version;
            foreach (var subElement in element.SubElements)
            {
                sum += AddUpVersion(subElement);
            }
            return sum;
        }

        private Element ReadElement(string binaryChain)
        {
            Element element = null;
            if (binaryChain.Replace("0", "").Length == 0)
            {
                return element;
            }

            element = CreateElementFromChain(binaryChain);
            if (element.Type == 4)
            {
                ReadLitteralValue(element, binaryChain.Substring(6));
            }
            else
            {
                ReadOperatorValue(element, binaryChain.Substring(6));
            }

            return element;
        }

        private Element CreateElementFromChain(string binaryChain)
        {
            return new Element()
            {
                Version = Convert.ToInt32(binaryChain.Substring(0, 3), 2),
                Type = Convert.ToInt32(binaryChain.Substring(3, 3), 2),
                Size = 6
            };
        }

        private void ReadLitteralValue(Element element, string litteral)
        {
            string value = "";
            for (int i = 0; i < litteral.Length; i = i + 5)
            {
                value += litteral.Substring(i + 1, 4);
                if (litteral.ToCharArray()[i] == '0')
                {
                    element.Size += i + 5;
                    break;
                }
            }
            element.Value = Convert.ToInt64(value, 2).ToString();
        }

        private void ReadOperatorValue(Element element, string operatorChain)
        {
            string value = "";
            int operatorType = int.Parse(operatorChain.Substring(0, 1));
            element.Size += 1;
            if (operatorType == 0)
            {
                int subpacketsSize = Convert.ToInt32(operatorChain.Substring(1, 15), 2);
                element.Size += 15 + subpacketsSize;
                element.SubElements = ReadSubpackets(operatorChain.Substring(16, subpacketsSize));
            }
            else
            {
                int countSubpackets = Convert.ToInt32(operatorChain.Substring(1, 11), 2);
                element.Size += 11;
                int increment = 0;
                for (int i = 0; i < countSubpackets; i++)
                {
                    element.SubElements.Add(ReadElement(operatorChain.Substring(12 + increment)));
                    increment += element.SubElements.Last().Size;
                    element.Size += element.SubElements.Last().Size;
                }
            }

            switch (element.Type)
            {
                case 0:
                    element.Value = element.SubElements.Select(x => Int64.Parse(x.Value)).Sum().ToString(); 
                    break;
                case 1:
                    element.Value = element.SubElements.Select(x => Int64.Parse(x.Value)).Aggregate((x, y) => x * y)
                        .ToString();
                    break;
                case 2:
                    element.Value = element.SubElements.Select(x => Int64.Parse(x.Value)).Min().ToString();
                    break;
                case 3:
                    element.Value = element.SubElements.Select(x => Int64.Parse(x.Value)).Max().ToString();
                    break;
                case 5:
                    element.Value = Int64.Parse(element.SubElements[0].Value) > Int64.Parse(element.SubElements[1].Value) ? "1" : "0";
                    break;
                case 6:
                    element.Value = Int64.Parse(element.SubElements[0].Value) < Int64.Parse(element.SubElements[1].Value) ? "1" : "0";
                    break;
                case 7:
                    element.Value = Int64.Parse(element.SubElements[0].Value) == Int64.Parse(element.SubElements[1].Value) ? "1" : "0";
                    break;
            }
        }

        private List<Element> ReadSubpackets(string subpackets)
        {
            List<Element> elements = new List<Element>();

            Element element;
            while ((element = ReadElement(subpackets)) != null)
            {
                elements.Add(element);
                subpackets = subpackets.Substring(element.Size);
            }

            return elements;
        }
    }
}
