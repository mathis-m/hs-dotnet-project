namespace UconsoleI.UI;

public interface IConsoleCursor
{
    void Show(bool show);
    void SetPosition(int column, int line);
    void Move(CursorDirection direction, int steps);
}