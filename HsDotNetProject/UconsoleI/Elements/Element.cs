using UconsoleI.Extensions;
using UconsoleI.Internal;
using UconsoleI.Stylings;

namespace UconsoleI.Elements;

public record Element
{
    public Element(string text, bool? isNewLine = false, bool? isWhiteSpace = false, bool? isControlCode = false, Styling? styling = null)
    {
        Text          = text?.NormalizeNewLines() ?? throw new ArgumentNullException(nameof(text));
        IsWhiteSpace  = isWhiteSpace ?? string.IsNullOrWhiteSpace(text);
        Styling       = styling ?? DefaultStylings.Plain;
        IsNewLine     = isNewLine ?? false;
        IsControlCode = isControlCode ?? false;
    }

    public string Text { get; init; }
    public bool IsNewLine { get; init; }
    public bool IsWhiteSpace { get; init; }
    public bool IsControlCode { get; init; }
    public Styling Styling { get; init; }

    public static Element Control(string control)
    {
        return new Element(control, isControlCode: true);
    }

    public void Deconstruct(out string text, out bool isNewLine, out bool isWhiteSpace, out bool isControlCode, out Styling styling)
    {
        text          = Text;
        isNewLine     = IsNewLine;
        isWhiteSpace  = IsWhiteSpace;
        isControlCode = IsControlCode;
        styling       = Styling;
    }

    public int CellCount()
    {
        return IsControlCode ? 0 : CellUtils.GetCellLength(Text);
    }

    public Element StripLineEndings()
    {
        return this with { Text = Text.TrimEnd('\n').TrimEnd('\r') };
    }

    public (Element First, Element? Second) Split(int offset)
    {
        if (offset < 0) return (this, null);

        if (offset == 0)
            return (
                this with { Text = string.Empty },
                this
            );

        if (offset >= CellCount()) return (this, null);

        var index = 0;

        var accumulated = 0;
        foreach (var character in Text)
        {
            index++;
            accumulated += CellUtils.GetCellLength(character);
            if (accumulated >= offset) break;
        }

        if (index < 0 || Text.Length <= index)
            throw new InvalidOperationException("");

        return (
            this with { Text = Text[..index] },
            this with { Text = Text[index..^index] }
        );
    }
}