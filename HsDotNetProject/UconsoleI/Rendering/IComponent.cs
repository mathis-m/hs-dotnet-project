using UconsoleI.Elements;

namespace UconsoleI.Rendering;

public interface IComponent
{
    SizeConstraint CalculateSizeConstraint(UIContext context, int maxWidth);

    IEnumerable<Element> Render(UIContext context, int maxWidth);
}