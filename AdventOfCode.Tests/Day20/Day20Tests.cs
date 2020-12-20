using System;
using System.IO;
using AdventOfCode.Solutions;
using AdventOfCode.Solutions.Year2020;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day20Tests
    {
        string EXAMPLE_FILEPATH = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"Day20/example"));
        string INPUT_FILEPATH = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"Day20/input"));
        
        [Fact]
        public void Part1_Example()
        {
            var input = File.ReadAllText(EXAMPLE_FILEPATH).Replace("\r\n", "\n");
            var _sut = new Day20(input);
            _sut.Part1.Should().Be("20899048083289");
        }
        
        
        [Fact]
        public void Part1_Actual()
        {
            var input = File.ReadAllText(INPUT_FILEPATH).Replace("\r\n", "\n\n");
            var _sut = new Day20(input);

            _sut.Part1.Should().Be("14129524957217");
        }

        // [Fact]
        // public void Part2_FinalPositions_Example()
        // {
        //     var input = File.ReadAllText(EXAMPLE_FILEPATH).Replace("\r\n", "\n");
        //     var _sut = new Day20(input);
        //
        //     var finalPos = _sut.GetFinalPositions();
        //     var correctCorner1 = finalPos[0, 0].Id == 1951 && finalPos[0, 2].Id == 2971 && finalPos[2, 0].Id == 3079 && finalPos[2, 2].Id == 1171;
        //
        //     correctCorner1.Should().BeTrue();
        // }

        [Fact]
        public void Part2_Example()
        {
            var input = File.ReadAllText(EXAMPLE_FILEPATH).Replace("\r\n", "\n");
            var _sut = new Day20(input);
            _sut.Part2.Should().Be("273");
        }
        
        [Fact]
        public void Part2_Actual()
        {
            var input = File.ReadAllText(INPUT_FILEPATH).Replace("\r\n", "\n");
            var _sut = new Day20(input);
            _sut.Part2.Should().Be("1649");
        }
    }
}