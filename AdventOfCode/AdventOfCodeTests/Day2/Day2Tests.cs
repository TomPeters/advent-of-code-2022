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
        Assert.Equal(15, Day2Puzzle.GetTotalScore(game, new Part1PlayerStrategy()));
    }

    [Fact]
    public void Part1_WorksForRealData()
    {
        var game = ParseInput(FileHelper.ReadFromFile("Day2", "RealData.txt"));
        Assert.Equal(11449, Day2Puzzle.GetTotalScore(game, new Part1PlayerStrategy()));
    }

    [Fact]
    public void Part2_WorksForSampleData()
    {
        var game = ParseInput(FileHelper.ReadFromFile("Day2", "Sample.txt"));
        Assert.Equal(12, Day2Puzzle.GetTotalScore(game, new Part2PlayerStrategy()));
    }

    [Fact]
    public void Part2_WorksForRealData()
    {
        var game = ParseInput(FileHelper.ReadFromFile("Day2", "RealData.txt"));
        Assert.Equal(13187, Day2Puzzle.GetTotalScore(game, new Part2PlayerStrategy()));
    }
    
    static Game ParseInput(string input)
    {
        var rounds = input.Split("\n").Select(roundInput =>
        {
            var shapesInput = roundInput.Split(" ");
            var opponentHandShapeString = shapesInput[0];
            var playersHandShapeString = shapesInput[1];
            return new Round(GetOpponentsHandShape(opponentHandShapeString),
                GetEncodedPlayersHandShape(playersHandShapeString));
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
    
    static EncodedPlayerInstruction GetEncodedPlayersHandShape(string input) =>
        input switch
        {
            "X" => EncodedPlayerInstruction.X,
            "Y" => EncodedPlayerInstruction.Y,
            "Z" => EncodedPlayerInstruction.Z,
            _ => throw new ArgumentOutOfRangeException(input)
        };
}