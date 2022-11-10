namespace UconsoleI.Rendering;

public sealed class RenderPipeline
{
    private readonly List<IRenderHook> _hooks;
    private readonly object            _lock;

    public RenderPipeline()
    {
        _hooks = new List<IRenderHook>();
        _lock  = new object();
    }

    public void Attach(IRenderHook hook)
    {
        lock (_lock)
        {
            _hooks.Add(hook);
        }
    }

    public void Detach(IRenderHook hook)
    {
        lock (_lock)
        {
            _hooks.Remove(hook);
        }
    }

    public IEnumerable<IComponent> Process(UIContext context, IEnumerable<IComponent> components)
    {
        lock (_lock)
        {
            var current                                                     = components;
            for (var index = _hooks.Count - 1; index >= 0; index--) current = _hooks[index].Process(context, current);

            return current;
        }
    }
}