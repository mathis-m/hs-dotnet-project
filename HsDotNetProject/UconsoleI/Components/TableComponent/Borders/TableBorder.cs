using System.Text;
using UconsoleI.Extensions;
using UconsoleI.Rendering;

namespace UconsoleI.Components.TableComponent.Borders;

public abstract class TableBorder
{
    public virtual bool Visible { get; } = true;
    public abstract string GetPart(TableBorderPart part);

    public virtual string GetColumnRow(TablePart part, IReadOnlyList<int> widths, IReadOnlyList<IColumn> columns)
    {
        if (widths is null) throw new ArgumentNullException(nameof(widths));

        if (columns is null) throw new ArgumentNullException(nameof(columns));

        var (left, center, separator, right) = GetTableParts(part);

        var builder = new StringBuilder();
        builder.Append(left);

        foreach (var (columnIndex, _, lastColumn, columnWidth) in widths.EnumerateWithContext())
        {
            var padding     = columns[columnIndex].Padding;
            var centerWidth = padding.GetLeftSafe() + columnWidth + padding.GetRightSafe();
            builder.Append(string.Concat(center.Repeat(centerWidth)));

            if (!lastColumn) builder.Append(separator);
        }

        builder.Append(right);
        return builder.ToString();
    }

    protected (string Left, string Center, string Separator, string Right) GetTableParts(TablePart part)
    {
        return part switch
        {
            TablePart.Top =>
                (GetPart(TableBorderPart.HeaderTopLeft), GetPart(TableBorderPart.HeaderTop),
                    GetPart(TableBorderPart.HeaderTopSeparator), GetPart(TableBorderPart.HeaderTopRight)),

            TablePart.HeaderSeparator =>
                (GetPart(TableBorderPart.HeaderBottomLeft), GetPart(TableBorderPart.HeaderBottom),
                    GetPart(TableBorderPart.HeaderBottomSeparator), GetPart(TableBorderPart.HeaderBottomRight)),

            TablePart.FooterSeparator =>
                (GetPart(TableBorderPart.FooterTopLeft), GetPart(TableBorderPart.FooterTop),
                    GetPart(TableBorderPart.FooterTopSeparator), GetPart(TableBorderPart.FooterTopRight)),

            TablePart.Bottom =>
                (GetPart(TableBorderPart.FooterBottomLeft), GetPart(TableBorderPart.FooterBottom),
                    GetPart(TableBorderPart.FooterBottomSeparator), GetPart(TableBorderPart.FooterBottomRight)),

            _ => throw new NotSupportedException("Unknown column row part"),
        };
    }
}