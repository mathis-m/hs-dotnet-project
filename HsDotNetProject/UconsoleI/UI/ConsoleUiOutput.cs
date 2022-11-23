using System.Text;
using UconsoleI.Extensions;
using UconsoleI.Internal;

namespace UconsoleI.UI;

public sealed class ConsoleUiOutput : IConsoleUIOutput
{
    public ConsoleUiOutput(TextWriter writer)
    {
        Writer = writer ?? throw new ArgumentNullException(nameof(writer));
    }

    public TextWriter Writer { get; }

    public bool IsTerminal
    {
        get
        {
            if (Writer.IsStandardOut()) return !Console.IsOutputRedirected;

            if (Writer.IsStandardError()) return !Console.IsErrorRedirected;

            return false;
        }
    }

    public int Width => ConsoleHelper.GetSafeWidth();

    public int Height => ConsoleHelper.GetSafeHeight();

    public void SetEncoding(Encoding encoding)
    {
        if (Writer.IsStandardOut() || Writer.IsStandardError()) Console.OutputEncoding = encoding;
    }
}