using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AdventOfCode.Utils;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day16Tests
    {
        private ITestOutputHelper _output;
        public Day16Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/test.txt";
            Day16 day = new Day16();

            // Act
            var result = day.TreatHexadecimal(FileReader.GetFileContent(filePath).ToList()[0]);

            // Assert
            result.Value.Should().Be("2021");
        }

        [Fact]
        public void Test2Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/test2.txt";
            Day16 day = new Day16();

            // Act
            var result = day.TreatHexadecimal(FileReader.GetFileContent(filePath).ToList()[0]);

            // Assert
            result.Value.Should().Be("1");
        }

        [Fact]
        public void Test3Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/test3.txt";
            Day16 day = new Day16();

            // Act
            var result = day.TreatHexadecimal(FileReader.GetFileContent(filePath).ToList()[0]);

            // Assert
            result.Value.Should().Be("3");
        }

        [Fact]
        public void Test4Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/test4.txt";
            Day16 day = new Day16();

            // Act
            var result = day.Compute(filePath);

            // Assert
            result.Should().Be(16);
        }

        [Fact]
        public void Test5Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/test5.txt";
            Day16 day = new Day16();

            // Act
            var result = day.Compute(filePath);

            // Assert
            result.Should().Be(12);
        }

        [Fact]
        public void Test6Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/test6.txt";
            Day16 day = new Day16();

            // Act
            var result = day.Compute(filePath);

            // Assert
            result.Should().Be(23);
        }

        [Fact]
        public void Test7Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/test7.txt";
            Day16 day = new Day16();

            // Act
            var result = day.Compute(filePath);

            // Assert
            result.Should().Be(31);
        }

        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath =
                $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/input.txt";
            Day16 day = new Day16();

            // Act
            long result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath =
                $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/input.txt";
            Day16 day = new Day16();

            // Act
            var result = day.Compute2(filePath);

            // Result
            _output.WriteLine(result);
        }
    }
}