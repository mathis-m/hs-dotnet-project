using UconsoleI.Elements;

namespace UconsoleI.Rendering;

public abstract class Component : IComponent
{
    SizeConstraint IComponent.CalculateSizeConstraint(UIContext context, int maxWidth)
    {
        return CalculateSizeConstraint(context, maxWidth);
    }

    IEnumerable<Element> IComponent.Render(UIContext context, int maxWidth)
    {
        return Render(context, maxWidth);
    }


    protected virtual SizeConstraint CalculateSizeConstraint(UIContext context, int maxWidth)
    {
        return new SizeConstraint(maxWidth, maxWidth);
    }

    protected abstract IEnumerable<Element> Render(UIContext context, int maxWidth);
}