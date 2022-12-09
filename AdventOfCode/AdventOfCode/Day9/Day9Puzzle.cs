namespace AdventOfCode.Day9;

public class Day9Puzzle
{
    public static int GetNumberOfPositionsVisitedByRopeTail(Input input)
    {
        var tail = new Tail();
        var head = new Head(tail);
        input.ApplyMotionsToHead(head);
        return tail.GetVisitedPositions().Distinct().Count();
    }
}

public class Head
{
    private readonly Tail _tail;
    public Position Position { get; private set; } = new Position(0, 0);

    public Head(Tail tail)
    {
        _tail = tail;
    }

    public void Move(Direction direction)
    {
        Position = Position.GetAdjacentPosition(direction);
        _tail.AdjustPosition(Position);
    }
}

public class Tail
{
    Position _position = new(0, 0);
    private readonly List<Position> _visitedPositions = new();

    public Tail()
    {
        // Include the initial starting position as a visited position
        _visitedPositions.Add(_position);
    }

    public void AdjustPosition(Position headPosition)
    {
        _position = GetPositionCloserToHeadIfRequired(headPosition);
        _visitedPositions.Add(_position);
    }

    Position GetPositionCloserToHeadIfRequired(Position headPosition)
    {
        if (Math.Abs(headPosition.X - _position.X) > 1)
        {
            var newXPosition = headPosition.X < _position.X ? _position.X - 1 : _position.X + 1;
            var newYPosition = headPosition.Y != _position.Y ? headPosition.Y : _position.Y;
            return new Position(newXPosition, newYPosition);
        }

        if (Math.Abs(headPosition.Y - _position.Y) > 1)
        {
            var newYPosition = headPosition.Y < _position.Y ? _position.Y - 1 : _position.Y + 1;
            var newXPosition = headPosition.X != _position.X ? headPosition.X : _position.X;
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