using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day2Tests
    {
        private ITestOutputHelper _output;
        public Day2Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day2/Resources/test.txt";
            Day2 day = new Day2();

            // Act
            int result = day.Compute(filePath, false);

            // Assert
            result.Should().Be(150);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day2/Resources/input.txt";
            Day2 day = new Day2();

            // Act
            int result = day.Compute(filePath, false);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day2/Resources/test.txt";
            Day2 day = new Day2();

            // Act
            int result = day.Compute(filePath, true);

            // Assert
            result.Should().Be(900);
        }


        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day2/Resources/input.txt";
            Day2 day = new Day2();

            // Act
            int result = day.Compute(filePath, true);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}