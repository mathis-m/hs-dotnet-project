using System.Text;

namespace UconsoleI.UI;

public interface IConsoleUIOutput
{
    TextWriter Writer { get; }
    bool IsTerminal { get; }
    int Width { get; }
    int Height { get; }
    void SetEncoding(Encoding encoding);
}