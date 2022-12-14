using UconsoleI.Stylings;

namespace UconsoleI.Internal;

internal static class AnsiDecorationBuilder
{
    public static IEnumerable<byte> GetAnsiCodes(Decoration decoration)
    {
        if ((decoration & Decoration.Bold) != 0) yield return 1;

        if ((decoration & Decoration.Dim) != 0) yield return 2;

        if ((decoration & Decoration.Italic) != 0) yield return 3;

        if ((decoration & Decoration.Underline) != 0) yield return 4;

        if ((decoration & Decoration.SlowBlink) != 0) yield return 5;

        if ((decoration & Decoration.RapidBlink) != 0) yield return 6;

        if ((decoration & Decoration.Invert) != 0) yield return 7;

        if ((decoration & Decoration.Conceal) != 0) yield return 8;

        if ((decoration & Decoration.StrikeThrough) != 0) yield return 9;
    }
}