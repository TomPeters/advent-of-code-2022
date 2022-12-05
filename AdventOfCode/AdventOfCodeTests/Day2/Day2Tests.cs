using System;
using System.Linq;
using AdventOfCode.Day2;
using Xunit;

namespace AdventOfCodeTests.Day2;

public class Day2Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var game = ParseInput(FileHelper.ReadFromFile("Day2", "Sample.txt"));
        Assert.Equal(15, Day2Puzzle.GetTotalScore(game));
    }

    [Fact]
    public void Part2_WorksForRealData()
    {
        var game = ParseInput(FileHelper.ReadFromFile("Day2", "RealData.txt"));
        Assert.Equal(11449, Day2Puzzle.GetTotalScore(game));
    }

    static Game ParseInput(string input)
    {
        var rounds = input.Split("\n").Select(roundInput =>
        {
            var shapesInput = roundInput.Split(" ");
            var opponentHandShapeString = shapesInput[0];
            var playersHandShapeString = shapesInput[1];
            return new Round(GetOpponentsHandShape(opponentHandShapeString),
                GetPlayersHandShape(playersHandShapeString));
        }).ToArray();
        return new Game(rounds);
    }

    static HandShape GetOpponentsHandShape(string input) =>
        input switch
        {
            "A" => HandShape.Rock,
            "B" => HandShape.Paper,
            "C" => HandShape.Scissors,
            _ => throw new ArgumentOutOfRangeException(input)
        };
    
    static HandShape GetPlayersHandShape(string input) =>
        input switch
        {
            "X" => HandShape.Rock,
            "Y" => HandShape.Paper,
            "Z" => HandShape.Scissors,
            _ => throw new ArgumentOutOfRangeException(input)
        };
}