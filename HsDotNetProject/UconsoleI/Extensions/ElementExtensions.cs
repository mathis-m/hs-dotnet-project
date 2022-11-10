using System.Text;
using UconsoleI.Elements;
using UconsoleI.Rendering;

namespace UconsoleI.Extensions;

public static class ElementExtensions
{
    public static int CellCount(this IEnumerable<Element> elements)
    {
        if (elements is null)
        {
            throw new ArgumentNullException(nameof(elements));
        }

        return elements.Sum(element => element.CellCount());
    }

    public static List<ElementCollection> SplitLines(this IEnumerable<Element> elements)
    {
        if (elements is null)
        {
            throw new ArgumentNullException(nameof(elements));
        }

        return elements.SplitLines(int.MaxValue);
    }

    public static List<ElementCollection> SplitLines(this IEnumerable<Element> elements, int maxWidth)
    {
        if (elements is null)
        {
            throw new ArgumentNullException(nameof(elements));
        }

        var lines = new List<ElementCollection>();
        var line  = new ElementCollection();

        var stack = new Stack<Element>(elements.Reverse());

        while (stack.Count > 0)
        {
            var element       = stack.Pop();
            var elementLength = element.CellCount();

            // Does this element make the line exceed the max width?
            var lineLength = line.CellCount();
            if (lineLength + elementLength > maxWidth)
            {
                var diff   = -(maxWidth - (lineLength + elementLength));
                var offset = element.Text.Length - diff;

                var (first, second) = element.Split(offset);

                line.Add(first);
                lines.Add(line);
                line = new ElementCollection();

                if (second != null)
                {
                    stack.Push(second);
                }

                continue;
            }

            // Does the element contains a newline?
            if (element.Text.Contains('\n'))
            {
                // Is it a new line?
                if (element.Text == "\n")
                {
                    if (line.Length != 0 || element.IsNewLine)
                    {
                        lines.Add(line);
                        line = new ElementCollection();
                    }

                    continue;
                }

                var text = element.Text;
                while (text != null)
                {
                    var parts = text.SplitLines();
                    if (parts.Length > 0)
                    {
                        if (parts[0].Length > 0)
                        {
                            line.Add(element with { Text = parts[0] });
                        }
                    }

                    if (parts.Length > 1)
                    {
                        if (line.Length > 0)
                        {
                            lines.Add(line);
                            line = new ElementCollection();
                        }

                        text = string.Concat(parts.Skip(1).Take(parts.Length - 1));
                    }
                    else
                    {
                        text = null;
                    }
                }
            }
            else
            {
                line.Add(element);
            }
        }

        if (line.Count > 0)
        {
            lines.Add(line);
        }

        return lines;
    }


    public static List<Element> SplitOverflow(this Element element, int maxWidth, Overflow? overflow = Overflow.Wrap)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        if (element.CellCount() <= maxWidth)
        {
            return new List<Element>(1) { element };
        }

        var zeroMaxWidthRequested = Math.Max(0, maxWidth - 1) == 0;

        return overflow switch
        {
            Overflow.Wrap => element.Text
                .SplitOnMaxWidth(maxWidth)
                .Select(str => element with { Text = str })
                .ToList(),

            Overflow.Hidden when zeroMaxWidthRequested => new List<Element> { element with { Text = string.Empty } },

            Overflow.Hidden => new List<Element> { element with { Text = element.Text[..maxWidth] } },

            Overflow.Ellipsis when zeroMaxWidthRequested => new List<Element> { element with { Text = "..." } },
            Overflow.Ellipsis => new List<Element> { element with { Text = element.Text[..(maxWidth - 1)] + "…" } },
            _ => new List<Element>(),
        };
    }

    public static IEnumerable<Element> Truncate(this IEnumerable<Element> segments, int maxWidth)
    {
        if (segments is null)
        {
            throw new ArgumentNullException(nameof(segments));
        }

        var result = new List<Element>();

        var totalWidth = 0;
        var enumerable = segments as Element[] ?? segments.ToArray();
        foreach (var segment in enumerable)
        {
            var segmentCellWidth = segment.CellCount();
            if (totalWidth + segmentCellWidth > maxWidth)
            {
                break;
            }

            result.Add(segment);
            totalWidth += segmentCellWidth;
        }

        if (result.Count == 0 && enumerable.Any())
        {
            var segment = enumerable.First().Truncate(maxWidth);
            if (segment != null)
            {
                result.Add(segment);
            }
        }

        return result;
    }

    public static Element? Truncate(this Element? segment, int maxWidth)
    {
        if (segment is null)
        {
            return null;
        }

        if (segment.CellCount() <= maxWidth)
        {
            return segment;
        }

        var builder = new StringBuilder();
        foreach (var character in segment.Text)
        {
            var accumulatedCellWidth = builder.ToString().GetCellWidth();
            if (accumulatedCellWidth >= maxWidth)
            {
                break;
            }

            builder.Append(character);
        }

        if (builder.Length == 0)
        {
            return null;
        }

        return segment with { Text = builder.ToString() };
    }
}