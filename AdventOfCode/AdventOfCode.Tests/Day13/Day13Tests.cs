using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day13Tests
    {
        private ITestOutputHelper _output;
        public Day13Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day13/Resources/test.txt";
            Day13 day = new Day13();

            // Act
            int result = day.Compute(filePath);

            // Assert
            result.Should().Be(17);
        }

        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day13/Resources/input.txt";
            Day13 day = new Day13();

            // Act
            int result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day13/Resources/input.txt";
            Day13 day = new Day13();

            // Act
            List<string> result = day.Compute2(filePath);

            // Result
            foreach(var line in result)
            {
                _output.WriteLine(line);
            }
        }
    }
}