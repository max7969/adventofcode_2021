using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day18Tests
    {
        private ITestOutputHelper _output;
        public Day18Tests(ITestOutputHelper output)
        {
            _output = output;
        }



        [Theory]
        [InlineData("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
        [InlineData("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
        [InlineData("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
        [InlineData("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
        [InlineData("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
        public void Test1Part1(string initialChain, string resultChain)
        {
            // Act
            string result = Day18.CycleReduce(initialChain);

            // Assert
            result.Should().Be(resultChain);
        }

        [Fact]
        public void Test2Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day18/Resources/test.txt";
            Day18 day = new Day18();

            // Act
            long result = day.Compute(filePath);

            // Assert
            result.Should().Be(3488);
        }

        [Fact]
        public void Test3Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day18/Resources/test2.txt";
            Day18 day = new Day18();

            // Act
            long result = day.Compute(filePath);

            // Assert
            result.Should().Be(4140);
        }

        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day18/Resources/input.txt";
            Day18 day = new Day18();

            // Act
            long result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day18/Resources/test2.txt";
            Day18 day = new Day18();

            // Act
            long result = day.Compute2(filePath);

            // Assert
            result.Should().Be(3993);
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day18/Resources/input.txt";
            Day18 day = new Day18();

            // Act
            long result = day.Compute2(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}