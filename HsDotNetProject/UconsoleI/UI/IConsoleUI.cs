using UconsoleI.Rendering;
using UconsoleI.Stylings;

namespace UconsoleI.UI;

public interface IConsoleUI
{
    IConsoleCursor Cursor { get; }
    IConsoleUIInput Input { get; }
    IExclusivityMode ExclusivityMode { get; }
    RenderPipeline Pipeline { get; }

    void Clear(bool home = true);
    void Write(IComponent component);
    void Write(string text, Styling? style = null);
}