using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day8Tests
    {
        private ITestOutputHelper _output;
        public Day8Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day8/Resources/test.txt";
            Day8 day = new Day8();

            // Act
            int result = day.Compute(filePath);

            // Assert
            result.Should().Be(26);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day8/Resources/input.txt";
            Day8 day = new Day8();

            // Act
            int result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day8/Resources/test2.txt";
            Day8 day = new Day8();

            // Act
            long result = day.Compute2(filePath);

            // Assert
            result.Should().Be(5353);
        }

        [Fact]
        public void Test2Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day8/Resources/test.txt";
            Day8 day = new Day8();

            // Act
            long result = day.Compute2(filePath);

            // Assert
            result.Should().Be(61229);
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day8/Resources/input.txt";
            Day8 day = new Day8();

            // Act
            long result = day.Compute2(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}