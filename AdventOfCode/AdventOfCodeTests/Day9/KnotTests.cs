using AdventOfCode.Day9;
using Xunit;

namespace AdventOfCodeTests.Day9;

public class KnotTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    [InlineData(1, 1)]
    [InlineData(1, -1)]
    [InlineData(-1, 0)]
    [InlineData(-1, 1)]
    [InlineData(-1, -1)]
    public void PositionIsCloseToHead_DoesntMove(int x, int y)
    {
        var knot = Knot.CreateTail();
        knot.AdjustPosition(new Position(x, y));
        Assert.Equal(new Position(0, 0), knot.Position);
    }
    
    [Theory]
    [InlineData(2, 0, 1, 0)]
    [InlineData(2, 1, 1, 1)]
    [InlineData(2, -1, 1, -1)]
    [InlineData(-2, 0, -1, 0)]
    [InlineData(-2, 1, -1, 1)]
    [InlineData(-2, -1, -1, -1)]
    [InlineData(0, 2, 0, 1)]
    [InlineData(1, 2, 1, 1)]
    [InlineData(-1, 2, -1, 1)]
    [InlineData(0, -2, 0, -1)]
    [InlineData(1, -2, 1, -1)]
    [InlineData(-1, -2, -1, -1)]
    public void PositionIsFurtherAwayFromHead_MovesCloserToNewPosition(int x, int y, int expectedNewX, int expectedNewY)
    {
        var knot = Knot.CreateTail();
        knot.AdjustPosition(new Position(x, y));
        Assert.Equal(new Position(expectedNewX, expectedNewY), knot.Position);
    }

    [Theory]
    [InlineData(3, 0, 1, 0)]
    [InlineData(-3, 0, -1, 0)]
    [InlineData(0, 4, 0, 1)]
    [InlineData(0, -5, 0, -1)]
    [InlineData(4, 4, 1, 1)]
    [InlineData(6, -5, 1, -1)]
    [InlineData(-2, -2, -1, -1)]
    [InlineData(-2, 2, -1, 1)]
    public void PositionIsEvenFurtherAwayFromHead_MovesCloser(int x, int y, int expectedNewX, int expectedNewY)
    {
        var knot = Knot.CreateTail();
        knot.AdjustPosition(new Position(x, y));
        Assert.Equal(new Position(expectedNewX, expectedNewY), knot.Position);
    }

    [Fact]
    public void KnotChain_MoveHead_MovesTail()
    {
        var tail = Knot.CreateTail();
        var midKnot = Knot.CreateIntermediateKnot(tail);
        var head = new Head(midKnot);

        head.Move(Direction.Right);
        head.Move(Direction.Right); // intermediate moves
        head.Move(Direction.Right); // tail + intermediate moves
        
        Assert.Equal(new Position(1, 0), tail.Position);

        head.Move(Direction.Down);
        head.Move(Direction.Down);
        
        Assert.Equal(new Position(2, -1), tail.Position);
    }
}