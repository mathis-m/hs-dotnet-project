using UconsoleI.Components.TableComponent.Borders;
using UconsoleI.Components.TextComponent;
using UconsoleI.Elements;
using UconsoleI.Rendering;
using UconsoleI.Stylings;

namespace UconsoleI.Components.TableComponent;

public sealed class Table : Component
{
    private readonly List<TableColumn> _columns;

    public Table()
    {
        _columns = new List<TableColumn>();
        Rows     = new TableRowCollection(this);
    }

    public IReadOnlyList<TableColumn> Columns => _columns;
    public TableRowCollection Rows { get; }

    public TableBorder Border { get; set; } = TableBorderTypes.Square;

    public Styling? BorderStyle { get; set; }

    public bool ShowHeaders { get; set; } = true;

    public bool ShowFooters { get; set; } = true;

    public bool Expand { get; set; }

    public int? Width { get; set; }

    public TableTitle? Title { get; set; }

    public TableTitle? Caption { get; set; }

    public Justify? Alignment { get; set; }

    internal bool IsGrid { get; set; }

    internal bool PadRightCell { get; set; } = true;

    public Table AddColumn(string column)
    {
        if (column is null) throw new ArgumentNullException(nameof(column));

        return AddColumn(new TableColumn(column));
    }

    public Table AddColumn(string column, Justify justification)
    {
        if (column is null) throw new ArgumentNullException(nameof(column));

        return AddColumn(new TableColumn(column, justification));
    }

    public Table AddColumn(TableColumn column)
    {
        if (column is null) throw new ArgumentNullException(nameof(column));

        if (Rows.Count > 0) throw new InvalidOperationException("Cannot add new columns to table with existing rows.");

        _columns.Add(column);
        return this;
    }

    public Table AddRow(params string[] columns)
    {
        if (columns is null) throw new ArgumentNullException(nameof(columns));

        return AddRow(columns.Select(column => new Text(column)).ToArray());
    }

    public Table AddRow(params IComponent[] columns)
    {
        if (columns is null) throw new ArgumentNullException(nameof(columns));
        return AddRow((IEnumerable<IComponent>) columns);
    }

    public Table AddRow(IEnumerable<IComponent> columns)
    {
        if (columns is null) throw new ArgumentNullException(nameof(columns));

        Rows.Add(new TableRow(columns));
        return this;
    }

    protected override SizeConstraint CalculateSizeConstraint(UIContext context, int maxWidth)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        var calculator = new TableSizeCalculator(this, context);

        var totalCellWidth = calculator.CalculateTotalCellWidth(maxWidth);

        var sizeConstraints = _columns.Select(column => calculator.CalculateColumnSizeConstraint(column, totalCellWidth)).ToList();
        var minTableWidth   = sizeConstraints.Sum(x => x.MinWidth) + calculator.GetNonColumnWidth();
        var maxTableWidth   = Width ?? sizeConstraints.Sum(x => x.MaxWidth) + calculator.GetNonColumnWidth();
        return new SizeConstraint(minTableWidth, maxTableWidth);
    }

    protected override IEnumerable<Element> Render(UIContext context, int maxWidth)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        var calculator = new TableSizeCalculator(this, context);

        var totalCellWidth = calculator.CalculateTotalCellWidth(maxWidth);
        var columnWidths   = calculator.CalculateColumnWidths(totalCellWidth);
        var tableWidth     = columnWidths.Sum() + calculator.GetNonColumnWidth();

        var rowsToRender = GetRenderableRows();

        return TableRenderer.Render(
            new TableRendererContext(this, context, rowsToRender, tableWidth, maxWidth),
            columnWidths);
    }

    private List<TableRow> GetRenderableRows()
    {
        var rows = new List<TableRow>();

        if (ShowHeaders) rows.Add(TableRow.Header(_columns.Select(c => c.Header)));

        rows.AddRange(Rows);

        var shouldRenderFooter = ShowFooters && _columns.Any(c => c.Footer != null);
        if (shouldRenderFooter) rows.Add(TableRow.Footer(_columns.Select(c => c.Footer ?? Text.Empty)));

        return rows;
    }
}