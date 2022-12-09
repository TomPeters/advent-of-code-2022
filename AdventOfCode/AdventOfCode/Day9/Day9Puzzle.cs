namespace AdventOfCode.Day9;

public class Day9Puzzle
{
    public static int GetNumberOfPositionsVisitedByRopeTail(Input input, int numberOfKnots)
    {
        var tail = Knot.CreateTail();
        var firstKnot = Enumerable.Range(0, numberOfKnots - 1).Aggregate(tail, (nextKnot, _) => Knot.CreateIntermediateKnot(nextKnot));
        var head = new Head(firstKnot);
        input.ApplyMotionsToHead(head);
        return tail.GetVisitedPositions().Distinct().Count();
    }
}

public class Head
{
    private readonly Knot _nextKnot;
    Position _position = new Position(0, 0);

    public Head(Knot nextKnot)
    {
        _nextKnot = nextKnot;
    }

    public void Move(Direction direction)
    {
        _position = _position.GetAdjacentPosition(direction);
        _nextKnot.AdjustPosition(_position);
    }
}

public class Knot
{
    private readonly Knot? _nextKnot;
    Position _position = new(0, 0);
    private readonly List<Position> _visitedPositions = new();

    public static Knot CreateTail()
    {
        return new Knot(null);
    }

    public static Knot CreateIntermediateKnot(Knot nextKnot)
    {
        return new Knot(nextKnot);
    }
    
    Knot(Knot? nextKnot)
    {
        _nextKnot = nextKnot;
        // Include the initial starting position as a visited position
        _visitedPositions.Add(_position);
    }

    public void AdjustPosition(Position positionOfKnotBeingFollowed)
    {
        _position = GetPositionCloserToHeadIfRequired(positionOfKnotBeingFollowed);
        _visitedPositions.Add(_position);
        _nextKnot?.AdjustPosition(_position);
    }

    Position GetPositionCloserToHeadIfRequired(Position positionOfKnotBeingFollowed)
    {
        if (Math.Abs(positionOfKnotBeingFollowed.X - _position.X) > 1)
        {
            var newXPosition = positionOfKnotBeingFollowed.X < _position.X ? _position.X - 1 : _position.X + 1;
            var newYPosition = positionOfKnotBeingFollowed.Y != _position.Y ? positionOfKnotBeingFollowed.Y : _position.Y;
            return new Position(newXPosition, newYPosition);
        }

        if (Math.Abs(positionOfKnotBeingFollowed.Y - _position.Y) > 1)
        {
            var newYPosition = positionOfKnotBeingFollowed.Y < _position.Y ? _position.Y - 1 : _position.Y + 1;
            var newXPosition = positionOfKnotBeingFollowed.X != _position.X ? positionOfKnotBeingFollowed.X : _position.X;
            return new Position(newXPosition, newYPosition);
        }

        return _position;
    }

    public IEnumerable<Position> GetVisitedPositions()
    {
        return _visitedPositions;
    }
}

public record Position(int X, int Y)
{
    public Position GetAdjacentPosition(Direction direction)
    {
        return direction switch
        {
            Direction.Right => this with { X = X + 1 },
            Direction.Left => this with { X = X - 1 },
            Direction.Up => this with { Y = Y + 1 },
            Direction.Down => this with { Y = Y - 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}

public record Input(Motion[] Motions)
{
    public void ApplyMotionsToHead(Head head)
    {
        Motions.ForEach(motion => motion.MoveHead(head));
    }
}

public record Motion(Direction Direction, int NumberOfSteps)
{
    public void MoveHead(Head head)
    {
        Enumerable.Range(0, NumberOfSteps).ForEach(_ =>
        {
            head.Move(Direction);
        });
    }
}

public enum Direction
{
    Right,
    Left,
    Up,
    Down
}