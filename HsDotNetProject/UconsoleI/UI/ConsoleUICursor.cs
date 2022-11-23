using UconsoleI.Components.ControlCodeComponent;
using UconsoleI.Internal;

namespace UconsoleI.UI;

internal sealed class ConsoleUICursor : IConsoleCursor
{
    private readonly IConsoleUI _console;

    public ConsoleUICursor(IConsoleUI console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
    }

    public void Show(bool show)
    {
        _console.Write(show ? new ControlCode(AnsiSequences.SM(AnsiSequences.DECTCEM)) : new ControlCode(AnsiSequences.RM(AnsiSequences.DECTCEM)));
    }

    public void Move(CursorDirection direction, int steps)
    {
        if (steps == 0) return;

        switch (direction)
        {
            case CursorDirection.Up:
                _console.Write(new ControlCode(AnsiSequences.CUU(steps)));
                break;
            case CursorDirection.Down:
                _console.Write(new ControlCode(AnsiSequences.CUD(steps)));
                break;
            case CursorDirection.Right:
                _console.Write(new ControlCode(AnsiSequences.CUF(steps)));
                break;
            case CursorDirection.Left:
                _console.Write(new ControlCode(AnsiSequences.CUB(steps)));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    public void SetPosition(int column, int line)
    {
        _console.Write(new ControlCode(AnsiSequences.CUP(line, column)));
    }
}