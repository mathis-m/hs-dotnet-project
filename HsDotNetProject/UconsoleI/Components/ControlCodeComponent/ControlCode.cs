using UconsoleI.Elements;
using UconsoleI.Rendering;

namespace UconsoleI.Components.ControlCodeComponent;

internal sealed class ControlCode : Component
{
    private readonly Element _element;

    public ControlCode(string control)
    {
        _element = DefaultElements.Control(control);
    }

    protected override SizeConstraint CalculateSizeConstraint(UIContext context, int maxWidth)
    {
        return new SizeConstraint(0, 0);
    }

    protected override IEnumerable<Element> Render(UIContext context, int maxWidth)
    {
        yield return _element;
    }
}