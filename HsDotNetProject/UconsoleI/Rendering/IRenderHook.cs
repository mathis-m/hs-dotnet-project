namespace UconsoleI.Rendering;

public interface IRenderHook
{
    IEnumerable<IComponent> Process(UIContext context, IEnumerable<IComponent> components);
}