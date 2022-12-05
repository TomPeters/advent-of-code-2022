namespace AdventOfCode.Day4;

public class Day4Puzzle
{
    public static int GetNumberOfAssignmentPairsWhereOneContainsTheOther(AssignmentPair[] assignmentPairs)
    {
        return assignmentPairs.Count(p => p.OneAssignmentContainsTheOther());
    }
}

public record AssignmentPair(Assignment FirstAssignment, Assignment SecondAssignment)
{
    public bool OneAssignmentContainsTheOther()
    {
        var firstSectionRange = FirstAssignment.GetSectionRange();
        var secondSectionRange = SecondAssignment.GetSectionRange();
        return firstSectionRange.FullyContains(secondSectionRange) || secondSectionRange.FullyContains(firstSectionRange);
    }
}

public record SectionRange(int[] Sections)
{
    public bool FullyContains(SectionRange otherSectionRange)
    {
        return Sections.All(s => otherSectionRange.Sections.Contains(s));
    }
}

public record Assignment(int StartingSection, int FinalSection)
{
    public SectionRange GetSectionRange()
    {
        return new SectionRange(EnumerableExtensions.BoundedRange(StartingSection, FinalSection).ToArray());
    } 
}