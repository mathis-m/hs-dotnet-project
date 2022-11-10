using UconsoleI.Stylings;

namespace UconsoleI.Elements;

public static class DefaultElements
{
    public static readonly Element LineBreak = new(Environment.NewLine, isNewLine: true);
    public static readonly Element Empty     = new(string.Empty);
    public static Element Padding(int size) => new(new string(' ', size));
    public static Element Control(string control) => new(control, styling: DefaultStylings.Plain, isControlCode: true);
}