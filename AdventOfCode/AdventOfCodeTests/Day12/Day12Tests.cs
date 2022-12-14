using System.Linq;
using AdventOfCode.Day12;
using Xunit;

namespace AdventOfCodeTests.Day12;

public class Day12Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day12", "Sample.txt"));
        Assert.Equal(31, Day12Puzzle.GetLengthOfShortestPath(input));
    }
    
    [Fact]
    public void Part1_WorksForRealData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day12", "RealData.txt"));
        Assert.Equal(352, Day12Puzzle.GetLengthOfShortestPath(input));
    }
    
    [Fact]
    public void Part2_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day12", "Sample.txt"));
        Assert.Equal(29, Day12Puzzle.GetLengthOfShortestPathFromAnyPotentialStartingSquare(input));
    }
    
    [Fact]
    public void Part2_WorksForRealData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day12", "RealData.txt"));
        Assert.Equal(345, Day12Puzzle.GetLengthOfShortestPathFromAnyPotentialStartingSquare(input));
    }


    static Heightmap ParseInput(string input)
    {
        var squareRows = input.Split("\n").Select(rowInput =>
         {
             return rowInput.Select(heightChar => new Square(heightChar)).ToArray();
         }).ToArray();

         var zipped = squareRows.Zip(squareRows.Skip(1), (treeRow, adjacentTreeRowBelow) => treeRow.Zip(adjacentTreeRowBelow));
         foreach (var (squareAbove, squareBelow) in zipped.SelectMany(g => g))
         {
             Square.Connect(squareAbove, squareBelow);
         }
         
         foreach (var squareRow in squareRows)
         {
             foreach (var (leftSquare, rightSquare) in squareRow.Zip(squareRow.Skip(1)))
             {
                 Square.Connect(leftSquare, rightSquare);
             }
         }

         var allSquares = squareRows.SelectMany(s => s).ToArray();

         return new Heightmap(allSquares);
    }
}