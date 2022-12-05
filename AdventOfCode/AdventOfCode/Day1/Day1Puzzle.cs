namespace AdventOfCode.Day1;

public static class Day1Puzzle
{
    public static int GetHighestCalorieCountForASingleElf(Elf[] elves)
    {
        var elfCarryingTheMostCalories = elves.MaxBy(e => e.GetTotalCalories());
        if (elfCarryingTheMostCalories is null) throw new Exception("No elf found with the most calories");
        return elfCarryingTheMostCalories.GetTotalCalories();
    }

    public static int GetTotalCaloriesCarriedByTop3Elves(Elf[] elves)
    {
        return elves.OrderByDescending(e => e.GetTotalCalories()).Take(3).Sum(e => e.GetTotalCalories());
    }
}


public record Elf(Food[] Food)
{
    public int GetTotalCalories() => Food.Sum(f => f.Calories);
}

public record Food(int Calories);