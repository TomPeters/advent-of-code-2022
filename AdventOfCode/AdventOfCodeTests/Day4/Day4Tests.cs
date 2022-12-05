using System.Linq;
using AdventOfCode.Day4;
using Xunit;

namespace AdventOfCodeTests.Day4;

public class Day4Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var assignmentPairs = ParseAssignmentPairs(FileHelper.ReadFromFile("Day4", "Sample.txt"));
        Assert.Equal(2, Day4Puzzle.GetNumberOfAssignmentPairsWhereOneContainsTheOther(assignmentPairs));
    }

    [Fact]
    public void Part1_WorksForRealData()
    {
        var assignmentPairs = ParseAssignmentPairs(FileHelper.ReadFromFile("Day4", "RealData.txt"));
        Assert.Equal(464, Day4Puzzle.GetNumberOfAssignmentPairsWhereOneContainsTheOther(assignmentPairs));
    }
    
    static AssignmentPair[] ParseAssignmentPairs(string input)
    {
        return input.Split("\n").Select(assignmentPairInput =>
        {
            var parts = assignmentPairInput.Split(",");
            var firstAssignment = ParseAssignment(parts[0]);
            var secondAssignment = ParseAssignment(parts[1]);
            return new AssignmentPair(firstAssignment, secondAssignment);
        }).ToArray();
    }

    static Assignment ParseAssignment(string assignmentInputString)
    {
        var parts = assignmentInputString.Split("-");
        var start = int.Parse(parts[0]);
        var finish = int.Parse(parts[1]);
        return new Assignment(start, finish);
    }
}