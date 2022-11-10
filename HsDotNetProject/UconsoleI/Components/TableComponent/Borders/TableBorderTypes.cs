namespace UconsoleI.Components.TableComponent.Borders;

public static class TableBorderTypes
{
    public static TableBorder None { get; } = new NoTableBorder();
    public static TableBorder Square { get; } = new SquareTableBorder();
    public static TableBorder Round { get; } = new RoundedTableBorder();
    public static TableBorder HeavyHeadTableBorder { get; } = new HeavyHeadTableBorder();
    public static TableBorder MinimalDoubleHeadTableBorder { get; } = new MinimalDoubleHeadTableBorder();
}