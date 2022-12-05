namespace AdventOfCode;

public class Day3Puzzle
{
    public static int GetSumOfPriorityOfSingleContentInBothCompartmentsOfEachRucksack(Rucksack[] rucksacks)
    {
        return rucksacks.Sum(r => r.GetSingleContentInBothCompartments().GetPriority());
    }

    public static int GetSumOfEachGroupsBadgePriority(ElfGroup[] elfGroups)
    {
        return elfGroups.Sum(g => g.GetSingleContentInAllRucksacks().GetPriority());
    }
}

public record ElfGroup(Rucksack[] Rucksacks)
{
    public Content GetSingleContentInAllRucksacks()
    {
        var allContentInAllRucksacks = Rucksacks.SelectMany(r => r.AllContents).Distinct();

        var contentThatAppearsInAllRucksacks = allContentInAllRucksacks.Single(content => Rucksacks.All(r => r.AllContents.Contains(content)));

        return contentThatAppearsInAllRucksacks;
    }
}

public record Rucksack(Compartment FirstCompartment, Compartment SecondCompartment)
{
    public Content GetSingleContentInBothCompartments()
    {
        var allContentTypes = AllContents.Distinct();

        var contentThatAppearsInBothCompartments =
            allContentTypes
                .Where(content => FirstCompartment.Contents.Contains(content))
                .Single(content => SecondCompartment.Contents.Contains(content));

        return contentThatAppearsInBothCompartments;
    }

    public Content[] AllContents => FirstCompartment.Contents.Concat(SecondCompartment.Contents).ToArray();
}

public record Compartment(Content[] Contents);

public class Content : IEquatable<Content>
{
    private readonly char _contentKey;

    public Content(char contentKey)
    {
        _contentKey = contentKey;
    }

    public bool Equals(Content? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _contentKey == other._contentKey;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Content)obj);
    }

    public override int GetHashCode()
    {
        return _contentKey.GetHashCode();
    }

    public int GetPriority()
    {
        return char.IsUpper(_contentKey) ? _contentKey - 38 : _contentKey - 96;
    }
}