using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day6Tests
    {
        private ITestOutputHelper _output;
        public Day6Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day6/Resources/test.txt";
            Day6 day = new Day6();

            // Act
            long result = day.Compute(filePath);

            // Assert
            result.Should().Be(5934);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day6/Resources/input.txt";
            Day6 day = new Day6();

            // Act
            long result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day6/Resources/test.txt";
            Day6 day = new Day6();

            // Act
            long result = day.Compute(filePath, 256);

            // Result
            result.Should().Be(26984457539);
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day6/Resources/input.txt";
            Day6 day = new Day6();

            // Act
            long result = day.Compute(filePath, 256);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}