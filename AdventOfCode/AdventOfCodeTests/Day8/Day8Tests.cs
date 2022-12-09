using System.Linq;
using AdventOfCode.Day8;
using Xunit;

namespace AdventOfCodeTests.Day8;

public class Day8Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day8", "Sample.txt"));
        Assert.Equal(21, Day8Puzzle.GetNumberOfVisibleTrees(input));
    }
    
    [Fact]
    public void Part2_WorksForRealData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day8", "RealData.txt"));
        Assert.Equal(1782, Day8Puzzle.GetNumberOfVisibleTrees(input));
    }

    static Forest ParseInput(string input)
    {
         var treeRows = input.Split("\n").Select(row =>
         {
             return row.Select(treeHeightChar =>
             {
                 var height = int.Parse(treeHeightChar.ToString());
                 return new Tree(height);
             }).ToArray();
         }).ToArray();

         var zipped = treeRows.Zip(treeRows.Skip(1), (treeRow, adjacentTreeRowBelow) => treeRow.Zip(adjacentTreeRowBelow));
         foreach (var (treeAbove, treeBelow) in zipped.SelectMany(g => g))
         {
             treeAbove.RegisterAdjacentTree(treeBelow, Direction.Bottom);
             treeBelow.RegisterAdjacentTree(treeAbove, Direction.Top);
         }
         
         foreach (var treeRow in treeRows)
         {
             foreach (var (leftTree, rightTree) in treeRow.Zip(treeRow.Skip(1)))
             {
                 leftTree.RegisterAdjacentTree(rightTree, Direction.Right);
                 rightTree.RegisterAdjacentTree(leftTree, Direction.Left);
             }
         }

         return new Forest(treeRows.SelectMany(treeRow => treeRow).ToArray());
    }
}