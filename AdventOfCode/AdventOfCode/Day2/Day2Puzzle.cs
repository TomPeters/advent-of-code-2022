namespace AdventOfCode.Day2;

public static class Day2Puzzle
{
    public static int GetTotalScore(Game input, IPlayerStrategy playerStrategy)
    {
        return input.GetTotalScore(playerStrategy);
    }
}

public record Game(Round[] Rounds)
{
    public int GetTotalScore(IPlayerStrategy playerStrategy) => Rounds.Sum(r => r.GetScore(playerStrategy));
}

public interface IPlayerStrategy
{
    HandShape GetPlayerHandShape(EncodedPlayerInstruction instruction, HandShape opponentsHandShape);
}

public class Part1PlayerStrategy : IPlayerStrategy
{
    public HandShape GetPlayerHandShape(EncodedPlayerInstruction instruction, HandShape opponentsHandShape) =>
        instruction switch
        {
            EncodedPlayerInstruction.X => HandShape.Rock,
            EncodedPlayerInstruction.Y => HandShape.Paper,
            EncodedPlayerInstruction.Z => HandShape.Scissors,
            _ => throw new ArgumentOutOfRangeException(nameof(instruction), instruction, null)
        };
}

public record Round(HandShape OpponentsHandShape, EncodedPlayerInstruction EncodedPlayerInstruction)
{
    public int GetScore(IPlayerStrategy playerStrategy)
    {
        var playersHandShape = playerStrategy.GetPlayerHandShape(EncodedPlayerInstruction, OpponentsHandShape);
        var result = GetResult(playersHandShape);
        return GetScoreForResult(result) + GetHandScore(playersHandShape);
    }

    static int GetHandScore(HandShape handShape)
    {
        return handShape switch
        {
            HandShape.Rock => 1,
            HandShape.Paper => 2,
            HandShape.Scissors => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(handShape), handShape, null)
        };
    }

    static int GetScoreForResult(Result result) =>
        result switch
        {
            Result.Lose => 0,
            Result.Draw => 3,
            Result.Win => 6,
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
    
    Result GetResult(HandShape playersHandShape)
    {
        return OpponentsHandShape switch
        {
            HandShape.Paper => playersHandShape switch
            {
                HandShape.Paper => Result.Draw,
                HandShape.Rock => Result.Lose,
                HandShape.Scissors => Result.Win
            },
            HandShape.Rock => playersHandShape switch
            {
                HandShape.Paper => Result.Win,
                HandShape.Rock => Result.Draw,
                HandShape.Scissors => Result.Lose
            },
            HandShape.Scissors => playersHandShape switch
            {
                HandShape.Paper => Result.Lose,
                HandShape.Rock => Result.Win,
                HandShape.Scissors => Result.Draw
            }
        };
    }

    enum Result
    {
        Lose,
        Draw,
        Win
    }
}

public enum EncodedPlayerInstruction
{
    X,
    Y,
    Z
}

public enum HandShape
{
    Rock,
    Paper,
    Scissors
}