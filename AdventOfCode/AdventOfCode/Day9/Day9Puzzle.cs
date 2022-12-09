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
    public Position Position { get; private set; } = new(0, 0);
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
        _visitedPositions.Add(Position);
    }

    public void AdjustPosition(Position positionOfKnotBeingFollowed)
    {
        Position = GetPositionCloserToHeadIfRequired(positionOfKnotBeingFollowed);
        _visitedPositions.Add(Position);
        _nextKnot?.AdjustPosition(Position);
    }

    Position GetPositionCloserToHeadIfRequired(Position positionOfKnotBeingFollowed)
    {
        if (Math.Abs(positionOfKnotBeingFollowed.X - Position.X) > 1)
        {
            var newXPosition = positionOfKnotBeingFollowed.X < Position.X ? Position.X - 1 : Position.X + 1;
            var newYPosition = positionOfKnotBeingFollowed.Y != Position.Y ? positionOfKnotBeingFollowed.Y : Position.Y;
            return new Position(newXPosition, newYPosition);
        }

        if (Math.Abs(positionOfKnotBeingFollowed.Y - Position.Y) > 1)
        {
            var newYPosition = positionOfKnotBeingFollowed.Y < Position.Y ? Position.Y - 1 : Position.Y + 1;
            var newXPosition = positionOfKnotBeingFollowed.X != Position.X ? positionOfKnotBeingFollowed.X : Position.X;
            return new Position(newXPosition, newYPosition);
        }

        return Position;
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