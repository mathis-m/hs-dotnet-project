using UconsoleI.Components.TableComponent.Borders;
using UconsoleI.Components.TextComponent;
using UconsoleI.Elements;
using UconsoleI.Extensions;
using UconsoleI.Internal;
using UconsoleI.Rendering;
using UconsoleI.Stylings;
using UconsoleI.Stylings.Coloring;

namespace UconsoleI.Components.TableComponent;

internal static class TableRenderer
{
    private static readonly Styling DefaultHeadingStyle = new(new Color(0xC0C0C0));
    private static readonly Styling DefaultCaptionStyle = new(new Color(0x808080));

    public static List<Element> Render(TableRendererContext context, List<int> columnWidths)
    {
        // Can't render the table?
        if (context.TableWidth <= 0 || context.TableWidth > context.MaxWidth || columnWidths.Any(c => c <= 0))
        {
            return new List<Element>(new[] { new Element("…", styling: context.BorderStyle) });
        }

        var result = new List<Element>();
        result.AddRange(RenderAnnotation(context, context.Title, DefaultHeadingStyle));

        // Iterate all rows
        foreach (var (index, isFirstRow, isLastRow, row) in context.Rows.EnumerateWithContext())
        {
            var cellHeight = 1;

            // Get the list of cells for the row and calculate the cell height
            var cells = new List<List<ElementCollection>>();
            foreach (var (columnIndex, _, _, (rowWidth, cell)) in columnWidths.Zip(row).EnumerateWithContext())
            {
                var justification = context.Columns[columnIndex].Alignment;
                var childContext  = context.Options with {  Justification = justification};

                var lines = cell.Render(childContext, rowWidth).SplitLines();
                cellHeight = Math.Max(cellHeight, lines.Count);
                cells.Add(lines);
            }

            // Show top of header?
            if (isFirstRow && context.ShowBorder)
            {
                var separator = Aligner.Align(context.Border.GetColumnRow(TablePart.Top, columnWidths, context.Columns), context.Alignment, context.MaxWidth);
                result.Add(new Element(separator, styling: context.BorderStyle));
                result.Add(DefaultElements.LineBreak);
            }

            // Show footer separator?
            if (context.ShowFooters && isLastRow && context.ShowBorder && context.HasFooters)
            {
                var textBorder = context.Border.GetColumnRow(TablePart.FooterSeparator, columnWidths, context.Columns);
                if (!string.IsNullOrEmpty(textBorder))
                {
                    var separator = Aligner.Align(textBorder, context.Alignment, context.MaxWidth);
                    result.Add(new Element(separator, styling: context.BorderStyle));
                    result.Add(DefaultElements.LineBreak);
                }
            }

            // Make cells the same shape
            cells = MakeSameHeight(cellHeight, cells);

            // Iterate through each cell row
            foreach (var cellRowIndex in Enumerable.Range(0, cellHeight))
            {
                var rowResult = new List<Element>();

                foreach (var (cellIndex, isFirstCell, isLastCell, cell) in cells.EnumerateWithContext())
                {
                    if (isFirstCell && context.ShowBorder)
                    {
                        // Show left column edge
                        var part = isFirstRow && context.ShowHeaders ? TableBorderPart.HeaderLeft : TableBorderPart.CellLeft;
                        rowResult.Add(new Element(context.Border.GetPart(part), styling: context.BorderStyle));
                    }

                    // Pad column on left side.
                    if (context.ShowBorder || context.IsGrid)
                    {
                        var leftPadding = context.Columns[cellIndex].Padding.GetLeftSafe();
                        if (leftPadding > 0)
                        {
                            rowResult.Add(new Element(new string(' ', leftPadding)));
                        }
                    }

                    // Add content
                    rowResult.AddRange(cell[cellRowIndex]);

                    // Pad cell content right
                    var length = cell[cellRowIndex].Sum(segment => segment.CellCount());
                    if (length < columnWidths[cellIndex])
                    {
                        rowResult.Add(new Element(new string(' ', columnWidths[cellIndex] - length)));
                    }

                    // Pad column on the right side
                    if (context.ShowBorder || (context.HideBorder && !isLastCell) || (context.HideBorder && isLastCell && context.IsGrid && context.PadRightCell))
                    {
                        var rightPadding = context.Columns[cellIndex].Padding.GetRightSafe();
                        if (rightPadding > 0)
                        {
                            rowResult.Add(new Element(new string(' ', rightPadding)));
                        }
                    }

                    if (isLastCell && context.ShowBorder)
                    {
                        // Add right column edge
                        var part = isFirstRow && context.ShowHeaders ? TableBorderPart.HeaderRight : TableBorderPart.CellRight;
                        rowResult.Add(new Element(context.Border.GetPart(part), styling: context.BorderStyle));
                    }
                    else if (context.ShowBorder)
                    {
                        // Add column separator
                        var part = isFirstRow && context.ShowHeaders ? TableBorderPart.HeaderSeparator : TableBorderPart.CellSeparator;
                        rowResult.Add(new Element(context.Border.GetPart(part), styling: context.BorderStyle));
                    }
                }

                // Align the row result.
                Aligner.Align(rowResult, context.Alignment, context.MaxWidth);

                // Is the row larger than the allowed max width?
                if (rowResult.CellCount() > context.MaxWidth)
                {
                    result.AddRange(rowResult.Truncate(context.MaxWidth));
                }
                else
                {
                    result.AddRange(rowResult);
                }

                result.Add(DefaultElements.LineBreak);
            }

            // Show header separator?
            if (isFirstRow && context.ShowBorder && context.ShowHeaders && context.HasRows)
            {
                var separator = Aligner.Align(context.Border.GetColumnRow(TablePart.HeaderSeparator, columnWidths, context.Columns), context.Alignment, context.MaxWidth);
                result.Add(new Element(separator, styling: context.BorderStyle));
                result.Add(DefaultElements.LineBreak);
            }

            // Show bottom of footer?
            if (isLastRow && context.ShowBorder)
            {
                var separator = Aligner.Align(context.Border.GetColumnRow(TablePart.Bottom, columnWidths, context.Columns), context.Alignment, context.MaxWidth);
                result.Add(new Element(separator, styling: context.BorderStyle));
                result.Add(DefaultElements.LineBreak);
            }
        }

        result.AddRange(RenderAnnotation(context, context.Caption, DefaultCaptionStyle));
        return result;
    }

    private static IEnumerable<Element> RenderAnnotation(TableRendererContext context, TableTitle? header, Styling defaultStyle)
    {
        if (header == null)
        {
            return Array.Empty<Element>();
        }

        var paragraph = new Text(header.Text, header.Styling ?? defaultStyle)
            .Alignment(Justify.Center)
            .Overflow(Overflow.Ellipsis);

        // Render the paragraphs
        var segments = new List<Element>();
        segments.AddRange(((IComponent) paragraph).Render(context.Options, context.TableWidth));

        // Align over the whole buffer area
        Aligner.Align(segments, context.Alignment, context.MaxWidth);

        segments.Add(DefaultElements.LineBreak);
        return segments;
    }

    internal static List<List<ElementCollection>> MakeSameHeight(int cellHeight, List<List<ElementCollection>> cells)
    {
        if (cells is null)
        {
            throw new ArgumentNullException(nameof(cells));
        }

        foreach (var cell in cells)
        {
            if (cell.Count < cellHeight)
            {
                while (cell.Count != cellHeight)
                {
                    cell.Add(new ElementCollection());
                }
            }
        }

        return cells;
    }

}