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