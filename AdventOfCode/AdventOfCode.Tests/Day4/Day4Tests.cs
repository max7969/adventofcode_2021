using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day4Tests
    {
        private ITestOutputHelper _output;
        public Day4Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day4/Resources/test.txt";
            Day4 day = new Day4();

            // Act
            int result = day.Compute(filePath, false);

            // Assert
            result.Should().Be(4512);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day4/Resources/input.txt";
            Day4 day = new Day4();

            // Act
            int result = day.Compute(filePath, false);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day4/Resources/test.txt";
            Day4 day = new Day4();

            // Act
            int result = day.Compute(filePath, true);

            // Assert
            result.Should().Be(1924);
        }


        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day4/Resources/input.txt";
            Day4 day = new Day4();

            // Act
            int result = day.Compute(filePath, true);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}