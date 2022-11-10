using UconsoleI.Components.TableComponent.Borders;
using UconsoleI.Extensions;
using UconsoleI.Internal;
using UconsoleI.Rendering;

namespace UconsoleI.Components.TableComponent;

internal sealed class TableSizeCalculator
{
    private readonly Table _table;
    public UIContext Options { get; }
    public IReadOnlyList<TableColumn> Columns => _table.Columns;
    public IReadOnlyList<TableRow> Rows => _table.Rows;
    public bool Expand => _table.Expand || _table.Width != null;

    private const int EdgeCount = 2;

    private readonly int?        _explicitWidth;
    private readonly TableBorder _border;
    private readonly bool        _padRightCell;

    public TableSizeCalculator(Table table, UIContext options)
    {
        _table         = table ?? throw new ArgumentNullException(nameof(table));
        _explicitWidth = table.Width;
        _border        = table.Border;
        _padRightCell  = table.PadRightCell;

        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public int CalculateTotalCellWidth(int maxWidth)
    {
        var totalCellWidth = maxWidth;
        if (_explicitWidth != null)
        {
            totalCellWidth = Math.Min(_explicitWidth.Value, maxWidth);
        }

        return totalCellWidth - GetNonColumnWidth();
    }

    public int GetNonColumnWidth()
    {
        var hideBorder = !_border.Visible;
        var separators = hideBorder ? 0 : Columns.Count - 1;
        var edges      = hideBorder ? 0 : EdgeCount;
        var padding    = Columns.Select(x => x.Padding?.Width ?? 0).Sum();

        if (!_padRightCell)
        {
            padding -= Columns.Last().Padding.GetRightSafe();
        }

        return separators + edges + padding;
    }

    public List<int> CalculateColumnWidths(int maxWidth)
    {
        var width_ranges = Columns.Select(column => CalculateColumnSizeConstraint(column, maxWidth)).ToArray();
        var widths       = width_ranges.Select(range => range.MaxWidth).ToList();

        var tableWidth = widths.Sum();
        if (tableWidth > maxWidth)
        {
            var wrappable = Columns.Select(c => !c.NoWrap).ToList();
            widths     = CollapseWidths(widths, wrappable, maxWidth);
            tableWidth = widths.Sum();

            // last resort, reduce columns evenly
            if (tableWidth > maxWidth)
            {
                var excessWidth = tableWidth - maxWidth;
                widths     = Ratio.Reduce(excessWidth, widths.Select(_ => 1).ToList(), widths, widths);
                tableWidth = widths.Sum();
            }
        }

        if (tableWidth < maxWidth && Expand)
        {
            var padWidths = Ratio.Distribute(maxWidth - tableWidth, widths);
            widths = widths.Zip(padWidths, (a, b) => (a, b)).Select(f => f.a + f.b).ToList();
        }

        return widths;
    }

    public SizeConstraint CalculateColumnSizeConstraint(TableColumn column, int maxWidth)
    {
        // Predetermined width?
        if (column.Width != null)
        {
            return new SizeConstraint(column.Width.Value, column.Width.Value);
        }

        var columnIndex = Columns.IndexOf(column);
        var rows        = Rows.Select(row => row[columnIndex]);

        var minWidths = new List<int>();
        var maxWidths = new List<int>();

        // Include columns (both header and footer) in measurement
        var headerSizeConstraint = column.Header.CalculateSizeConstraint(Options, maxWidth);
        var (footerMinWidth, footerMaxWidth) = column.Footer?.CalculateSizeConstraint(Options, maxWidth) ?? headerSizeConstraint;
        minWidths.Add(Math.Min(headerSizeConstraint.MinWidth, footerMinWidth));
        maxWidths.Add(Math.Max(headerSizeConstraint.MaxWidth, footerMaxWidth));

        foreach (var row in rows)
        {
            var (rowMinWidth, rowMaxWidth) = row.CalculateSizeConstraint(Options, maxWidth);
            minWidths.Add(rowMinWidth);
            maxWidths.Add(rowMaxWidth);
        }

        var padding = column.Padding?.Width ?? 0;

        return new SizeConstraint(
            minWidths.Count > 0 ? minWidths.Max() : padding,
            maxWidths.Count > 0 ? maxWidths.Max() : maxWidth);
    }

    private static List<int> CollapseWidths(List<int> widths, List<bool> wrappable, int maxWidth)
    {
        var totalWidth  = widths.Sum();
        var excessWidth = totalWidth - maxWidth;

        var anyIsWrappable = wrappable.Any(x => x);
        if (!anyIsWrappable) return widths;

        while (totalWidth != 0 && excessWidth > 0)
        {
            var maxColumn = widths.Zip(wrappable, (first, second) => (width: first, allowWrap: second))
                .Where(x => x.allowWrap)
                .Max(x => x.width);

            var secondMaxColumn  = widths.Zip(wrappable, (width, allowWrap) => allowWrap && width != maxColumn ? width : 1).Max();
            var columnDifference = maxColumn - secondMaxColumn;

            var ratios = widths.Zip(wrappable, (width, allowWrap) => width == maxColumn && allowWrap ? 1 : 0).ToList();
            if (ratios.All(x => x == 0) || columnDifference == 0)
            {
                break;
            }

            var maxReduce = widths.Select(_ => Math.Min(excessWidth, columnDifference)).ToList();
            widths = Ratio.Reduce(excessWidth, ratios, maxReduce, widths);

            totalWidth  = widths.Sum();
            excessWidth = totalWidth - maxWidth;
        }

        return widths;
    }
}