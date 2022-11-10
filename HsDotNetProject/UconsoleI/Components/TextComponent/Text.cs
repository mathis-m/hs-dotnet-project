using UconsoleI.Components.ParagraphComponent;
using UconsoleI.Elements;
using UconsoleI.Rendering;
using UconsoleI.Stylings;

namespace UconsoleI.Components.TextComponent;

public sealed class Text : Component, IAlignable, IOverflowable
{
    private readonly Paragraph _paragraph;

    public static Text Empty { get; } = new Text(string.Empty);

    public static Text NewLine { get; } = new Text(Environment.NewLine, DefaultStylings.Plain);

    public Text(string text, Styling? styling = null)
    {
        _paragraph = new Paragraph(text, styling);
    }

    public Justify? Alignment
    {
        get => _paragraph.Alignment;
        set => _paragraph.Alignment = value;
    }

    public Overflow? Overflow
    {
        get => _paragraph.Overflow;
        set => _paragraph.Overflow = value;
    }

    public int Length => _paragraph.Length;

    public int Lines => _paragraph.Lines;

    protected override SizeConstraint CalculateSizeConstraint(UIContext context, int maxWidth)
    {
        return ((IComponent) _paragraph).CalculateSizeConstraint(context, maxWidth);
    }

    protected override IEnumerable<Element> Render(UIContext context, int maxWidth)
    {
        return ((IComponent) _paragraph).Render(context, maxWidth);
    }
}