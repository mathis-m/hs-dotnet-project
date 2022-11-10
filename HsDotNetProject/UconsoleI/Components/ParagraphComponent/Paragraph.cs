using UconsoleI.Elements;
using UconsoleI.Extensions;
using UconsoleI.Internal;
using UconsoleI.Rendering;
using UconsoleI.Stylings;

namespace UconsoleI.Components.ParagraphComponent;

public sealed class Paragraph : Component, IAlignable, IOverflowable
{
    private readonly List<ElementCollection> _lines;

    public Paragraph()
    {
        _lines = new List<ElementCollection>();
    }

    public Paragraph(string text, Styling? style = null)
        : this()
    {
        if (text is null) throw new ArgumentNullException(nameof(text));

        Append(text, style);
    }

    public int Length => _lines.Sum(line => line.Length) + Math.Max(0, Lines - 1);
    public int Lines => _lines.Count;
    public Justify? Alignment { get; set; }
    public Overflow? Overflow { get; set; }

    public Paragraph Append(string text, Styling? style = null)
    {
        if (text is null) throw new ArgumentNullException(nameof(text));

        foreach (var (_, isFirst, isLast, current) in text.SplitLines().EnumerateWithContext())
        {
            if (isFirst)
            {
                var line = _lines.LastOrDefault();
                if (line == null)
                {
                    _lines.Add(new ElementCollection());
                    line = _lines.Last();
                }

                if (string.IsNullOrEmpty(current))
                    line.Add(DefaultElements.Empty);
                else
                    line.AddRange(current.SplitWords().Select(span => new Element(span, styling: style ?? DefaultStylings.Plain)));
            }
            else
            {
                var line = new ElementCollection();

                if (string.IsNullOrEmpty(current))
                    line.Add(DefaultElements.Empty);
                else
                    line.AddRange(current.SplitWords().Select(span => new Element(span, styling: style ?? DefaultStylings.Plain)));

                _lines.Add(line);
            }
        }

        return this;
    }

    protected override SizeConstraint CalculateSizeConstraint(UIContext context, int maxWidth)
    {
        if (_lines.Count == 0) return new SizeConstraint(0, 0);

        var min = _lines.Max(line => line.Max(element => element.CellCount()));
        var max = _lines.Max(x => x.CellCount());

        return new SizeConstraint(min, Math.Min(max, maxWidth));
    }

    protected override IEnumerable<Element> Render(UIContext context, int maxWidth)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        if (_lines.Count == 0) return Array.Empty<Element>();

        var lines = context.SingleLine
            ? new List<ElementCollection>(_lines)
            : SplitLines(maxWidth);

        // Justify lines
        var justification = context.Justification ?? Alignment ?? Justify.Left;

        if (justification == Justify.Left)
            return context.SingleLine
                ? lines[0].Where(element => !element.IsNewLine)
                : new ElementCollectionEnumerator(lines);

        foreach (var line in lines) Aligner.Align(line, justification, maxWidth);

        return context.SingleLine
            ? lines[0].Where(element => !element.IsNewLine)
            : new ElementCollectionEnumerator(lines);
    }

    private List<ElementCollection> Clone()
    {
        var result = new List<ElementCollection>();

        foreach (var line in _lines)
        {
            var newLine = new ElementCollection();
            newLine.AddRange(line);
            result.Add(newLine);
        }

        return result;
    }

    private List<ElementCollection> SplitLines(int maxWidth)
    {
        if (maxWidth <= 0)
            // Nothing fits, so return an empty line.
            return new List<ElementCollection>();

        if (_lines.Max(x => x.CellCount()) <= maxWidth) return Clone();

        var lines = new List<ElementCollection>();
        var line  = new ElementCollection();

        using var iterator = new ElementCollectionIterator(_lines);
        var       queue    = new Queue<Element>();
        while (true)
        {
            Element? current;
            if (queue.Count == 0)
            {
                if (!iterator.MoveNext()) break;

                current = iterator.Current;
            }
            else
            {
                current = queue.Dequeue();
            }

            if (current == null) throw new InvalidOperationException("Iterator returned empty segment.");

            var newLine = false;

            if (current.IsNewLine)
            {
                lines.Add(line);
                line = new ElementCollection();
                continue;
            }

            var length      = current.CellCount();
            var hasOverflow = length > maxWidth;
            if (hasOverflow)
            {
                var elements = current.SplitOverflow(maxWidth, Overflow);
                if (elements.Count > 0)
                {
                    if (line.CellCount() + elements[0].CellCount() > maxWidth)
                    {
                        lines.Add(line);
                        line = new ElementCollection();

                        elements.ForEach(s => queue.Enqueue(s));
                        continue;
                    }

                    // Add the element and push the rest of them to the queue.
                    line.Add(elements[0]);
                    foreach (var element in elements.Skip(1)) queue.Enqueue(element);

                    continue;
                }
            }
            else
            {
                if (line.CellCount() + length > maxWidth)
                {
                    line.Add(DefaultElements.Empty);
                    lines.Add(line);
                    line    = new ElementCollection();
                    newLine = true;
                }
            }

            if (newLine && current.IsWhiteSpace) continue;

            line.Add(current);
        }

        var flushLeftItems = line.Count > 0;
        if (flushLeftItems) lines.Add(line);

        return lines;
    }
}