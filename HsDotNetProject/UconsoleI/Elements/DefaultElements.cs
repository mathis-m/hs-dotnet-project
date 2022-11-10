using UconsoleI.Stylings;

namespace UconsoleI.Elements;

public static class DefaultElements
{
    public static readonly Element LineBreak = new(Environment.NewLine, true);
    public static readonly Element Empty     = new(string.Empty);

    public static Element Padding(int size)
    {
        return new(new string(' ', size));
    }

    public static Element Control(string control)
    {
        return new(control, styling: DefaultStylings.Plain, isControlCode: true);
    }
}