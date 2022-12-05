namespace AdventOfCode.Day2;

public static class Day2Puzzle
{
    public static int GetTotalScore(Game input)
    {
        return input.GetTotalScore();
    }
}

public record Game(Round[] Rounds)
{
    public int GetTotalScore() => Rounds.Sum(r => r.GetScore());
}

public record Round(HandShape OpponentsHandShape, HandShape PlayersHandShape)
{
    public int GetScore()
    {
        var result = GetResult();
        return GetScoreForResult(result) + GetHandScore(PlayersHandShape);
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
    
    Result GetResult()
    {
        return OpponentsHandShape switch
        {
            HandShape.Paper => PlayersHandShape switch
            {
                HandShape.Paper => Result.Draw,
                HandShape.Rock => Result.Lose,
                HandShape.Scissors => Result.Win
            },
            HandShape.Rock => PlayersHandShape switch
            {
                HandShape.Paper => Result.Win,
                HandShape.Rock => Result.Draw,
                HandShape.Scissors => Result.Lose
            },
            HandShape.Scissors => PlayersHandShape switch
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

public enum HandShape
{
    Rock,
    Paper,
    Scissors
}