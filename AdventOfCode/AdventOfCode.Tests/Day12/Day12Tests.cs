using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day12Tests
    {
        private ITestOutputHelper _output;
        public Day12Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day12/Resources/test.txt";
            Day12 day = new Day12();

            // Act
            int result = day.Compute(filePath);

            // Assert
            result.Should().Be(10);
        }

        [Fact]
        public void Test2Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day12/Resources/test2.txt";
            Day12 day = new Day12();

            // Act
            int result = day.Compute(filePath);

            // Assert
            result.Should().Be(19);
        }

        [Fact]
        public void Test3Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day12/Resources/test3.txt";
            Day12 day = new Day12();

            // Act
            int result = day.Compute(filePath);

            // Assert
            result.Should().Be(226);
        }

        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day12/Resources/input.txt";
            Day12 day = new Day12();

            // Act
            int result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }


        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day12/Resources/test.txt";
            Day12 day = new Day12();

            // Act
            int result = day.Compute2(filePath);

            // Assert
            result.Should().Be(36);
        }

        [Fact]
        public void Test2Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day12/Resources/test2.txt";
            Day12 day = new Day12();

            // Act
            int result = day.Compute2(filePath);

            // Assert
            result.Should().Be(103);
        }

        [Fact]
        public void Test3Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day12/Resources/test3.txt";
            Day12 day = new Day12();

            // Act
            int result = day.Compute2(filePath);

            // Assert
            result.Should().Be(3509);
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day12/Resources/input.txt";
            Day12 day = new Day12();

            // Act
            int result = day.Compute2(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}