using UconsoleI.Rendering;

namespace UconsoleI.UI;

public interface IConsoleUI
{
    IConsoleCursor Cursor { get; }

    RenderPipeline Pipeline { get; }
    void Clear(bool home);

    void Write(IComponent component);
}