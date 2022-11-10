using UconsoleI.Components.TableComponent.Borders;
using UconsoleI.Rendering;
using UconsoleI.Stylings;

namespace UconsoleI.Components.TableComponent;

public record TableRendererContext
{
    private readonly Table          _table;
    private readonly List<TableRow> _rows;

    public IReadOnlyList<TableRow> Rows => _rows;

    public TableBorder Border { get; }
    public Styling BorderStyle { get; }
    public bool ShowBorder { get; }
    public bool HasRows { get; }
    public bool HasFooters { get; }

    public UIContext Options { get; }
    public IReadOnlyList<TableColumn> Columns => _table.Columns;
    public bool Expand => _table.Expand || _table.Width != null;
    public int MaxWidth { get; }
    public int TableWidth { get; }

    public bool HideBorder => !ShowBorder;
    public bool ShowHeaders => _table.ShowHeaders;
    public bool ShowFooters => _table.ShowFooters;
    public bool IsGrid => _table.IsGrid;
    public bool PadRightCell => _table.PadRightCell;
    public TableTitle? Title => _table.Title;
    public TableTitle? Caption => _table.Caption;
    public Justify? Alignment => _table.Alignment;

    public TableRendererContext(Table table, UIContext options, IEnumerable<TableRow> rows, int tableWidth, int maxWidth)
    {
        _table = table ?? throw new ArgumentNullException(nameof(table));
        _rows  = new List<TableRow>(rows ?? Enumerable.Empty<TableRow>());

        ShowBorder  = _table.Border.Visible;
        HasRows     = Rows.Any(row => !row.IsHeader && !row.IsFooter);
        HasFooters  = Rows.Any(column => column.IsFooter);
        Border      = table.Border;
        BorderStyle = table.BorderStyle ?? DefaultStylings.Plain;

        TableWidth = tableWidth;
        MaxWidth   = maxWidth;
        Options    = options ?? throw new ArgumentNullException(nameof(options));
    }
}