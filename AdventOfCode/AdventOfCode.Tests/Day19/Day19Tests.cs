﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day19Tests
    {
        private ITestOutputHelper _output;
        public Day19Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day19/Resources/test.txt";
            Day19 day = new Day19();

            // Act
            long result = day.Compute(filePath);

            // Assert
            result.Should().Be(79);
        }

        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day19/Resources/input.txt";
            Day19 day = new Day19();

            // Act
            long result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day19/Resources/test.txt";
            Day19 day = new Day19();

            // Act
            long result = day.Compute2(filePath);

            // Assert
            result.Should().Be(79);
        }
    }
}