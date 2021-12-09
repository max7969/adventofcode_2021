using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day9Tests
    {
        private ITestOutputHelper _output;
        public Day9Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day9/Resources/test.txt";
            Day9 day = new Day9();

            // Act
            int result = day.Compute(filePath);

            // Assert
            result.Should().Be(15);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day9/Resources/input.txt";
            Day9 day = new Day9();

            // Act
            int result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day9/Resources/test.txt";
            Day9 day = new Day9();

            // Act
            int result = day.Compute2(filePath);

            // Assert
            result.Should().Be(1134);
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day9/Resources/input.txt";
            Day9 day = new Day9();

            // Act
            int result = day.Compute2(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}