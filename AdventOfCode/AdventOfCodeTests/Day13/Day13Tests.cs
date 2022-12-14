using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Day13;
using Xunit;

namespace AdventOfCodeTests.Day13;

public class Day13Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day13", "Sample.txt"));
        Assert.Equal(13, Day13Puzzle.GetSumOfIndicesOfCorrectlyOrderedPairs(input));
    }

    [Fact]
    public void Part1_WorksForRealData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day13", "RealData.txt"));
        Assert.Equal(5292, Day13Puzzle.GetSumOfIndicesOfCorrectlyOrderedPairs(input));
    }
    
    [Fact]
    public void Part2_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day13", "Sample.txt"));
        Assert.Equal(140, Day13Puzzle.GetDecoderKey(input));
    }
    
    [Fact]
    public void Part2_WorksForRealData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day13", "RealData.txt"));
        Assert.Equal(23868, Day13Puzzle.GetDecoderKey(input));
    }
    
    private PacketPair[] ParseInput(string input)
    {
        return input.Split("\n\n").Select(pairInput =>
        {
            var packets = pairInput.Split("\n").Select(ParseCompletePacket).ToArray();
            return new PacketPair(packets[0], packets[1]);
        }).ToArray();
    }

    IPacket ParseCompletePacket(string input)
    {
        if (int.TryParse(input, out var parsedInt))
        {
            return new IntegerPacket(parsedInt);
        }

        var contentsExcludingEndBrackets = input.Substring(1, input.Length - 2);
        var packetStrings = SplitByTopLevelComma(contentsExcludingEndBrackets);
        var subPackets = packetStrings.Select(ParseCompletePacket);
        return new ListPacket(subPackets.ToArray());
    }

    IEnumerable<string> SplitByTopLevelComma(string input)
    {
        var bracketsCount = 0;
        var currentPacketString = "";
        foreach(var currentChar in input)
        {
            if (currentChar == ',' && bracketsCount == 0)
            {
                yield return currentPacketString;
                currentPacketString = "";
            }
            else
            {
                if (currentChar == '[') bracketsCount++;
                if (currentChar == ']') bracketsCount--;
                currentPacketString += currentChar;
            }
        }

        if (currentPacketString != "")
        {
            yield return currentPacketString;
        }
    } 
}