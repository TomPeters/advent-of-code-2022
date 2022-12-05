namespace AdventOfCode;

public static class StringExtensions
{
    public static string Transpose(this string input)
    {
        var positions = new List<StringPosition>();
        input.Split("\n").ForEach((rowInput, rowIndex) =>
        {
            rowInput.ForEach((c, columnIndex) =>
            {
                positions.Add(new StringPosition(columnIndex, rowIndex, c));
            });
        });

        var newPositions = positions.Select(p => new StringPosition(p.RowIndex, p.ColumnIndex, p.Input));

        var rows = newPositions.GroupBy(p => p.RowIndex);
        var rowStrings = rows.Select(r => new string(r.OrderBy(r => r.ColumnIndex).Select(c => c.Input).ToArray()));
        return string.Join("\n", rowStrings);
    }

    record StringPosition(int ColumnIndex, int RowIndex, char Input);
}